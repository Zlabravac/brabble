using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HungerBarWithSlider : MonoBehaviour
{
    [Header("Hunger Bar Settings")]
    public Slider hungerSlider; // Slider for the hunger bar
    public CanvasGroup hungerCanvasGroup; // CanvasGroup for fading the hunger bar
    public Image fillImage; // The Image component of the slider's fill area
    public float maxHunger = 100f; // Maximum hunger value
    public float hungerDecreaseRate = 5f; // Rate at which hunger decreases per second
    public float fadeDuration = 0.5f; // Duration for fading the bar
    public float lowHungerThreshold = 15f; // Percentage below which the bar fades out
    public float fadeBufferTime = 0.2f; // Shorter buffer time to reduce flickering

    [Header("Hunger Bar Colors")]
    public Color fullHungerColor = Color.green; // Color at full hunger
    public Color halfHungerColor = Color.yellow; // Color at half hunger
    public Color lowHungerColor = Color.red; // Color at low hunger

    private float currentHunger; // Current hunger value
    private bool isFading = false; // Tracks if fading is happening
    private float fadeBufferTimer = 0f; // Timer for the fade buffer

    public float GetHungerPercentage()
    {
        return (currentHunger / maxHunger) * 100f;
    }

    void Start()
    {
        // Initialize hunger to maximum at the start
        currentHunger = maxHunger;
        UpdateHungerBar();
    }

    void Update()
    {
        // Decrease hunger over time
        currentHunger -= hungerDecreaseRate * Time.deltaTime;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);

        // Update the hunger bar slider
        UpdateHungerBar();

        // Handle fading based on hunger level with buffer
        HandleHungerBarVisibility();
    }

    private void UpdateHungerBar()
    {
        if (hungerSlider != null)
        {
            hungerSlider.value = currentHunger / maxHunger; // Update the slider value

            // Update the fill color based on the hunger level
            UpdateFillColor();
        }
    }

    private void UpdateFillColor()
    {
        if (fillImage == null)
            return;

        float hungerPercentage = GetHungerPercentage();

        if (hungerPercentage > 50f)
        {
            // Transition from green to yellow
            fillImage.color = Color.Lerp(halfHungerColor, fullHungerColor, (hungerPercentage - 50f) / 50f);
        }
        else
        {
            // Transition from yellow to red
            fillImage.color = Color.Lerp(lowHungerColor, halfHungerColor, hungerPercentage / 50f);
        }
    }

    private void HandleHungerBarVisibility()
    {
        if (hungerCanvasGroup == null)
            return;

        float hungerPercentage = GetHungerPercentage();

        if ((hungerPercentage < lowHungerThreshold && hungerCanvasGroup.alpha > 0) ||
            (hungerPercentage >= lowHungerThreshold && hungerCanvasGroup.alpha < 1))
        {
            fadeBufferTimer += Time.deltaTime;
            if (fadeBufferTimer >= fadeBufferTime)
            {
                if (hungerPercentage < lowHungerThreshold && !isFading)
                {
                    StartCoroutine(FadeOut());
                }
                else if (hungerPercentage >= lowHungerThreshold && !isFading)
                {
                    StartCoroutine(FadeIn());
                }
                fadeBufferTimer = 0f; // Reset buffer timer after fade
            }
        }
        else
        {
            fadeBufferTimer = 0f; // Reset if condition is not met
        }
    }

    private IEnumerator FadeOut()
    {
        isFading = true;
        float startAlpha = hungerCanvasGroup.alpha;
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            hungerCanvasGroup.alpha = Mathf.Lerp(startAlpha, 0, elapsedTime / fadeDuration);
            yield return null;
        }

        hungerCanvasGroup.alpha = 0;
        isFading = false;
    }

    private IEnumerator FadeIn()
    {
        isFading = true;
        float startAlpha = hungerCanvasGroup.alpha;
        float elapsedTime = 0;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            hungerCanvasGroup.alpha = Mathf.Lerp(startAlpha, 1, elapsedTime / fadeDuration);
            yield return null;
        }

        hungerCanvasGroup.alpha = 1;
        isFading = false;
    }

    public void ModifyHunger(float amount)
    {
        // Modify the hunger value and clamp it between 0 and maxHunger
        currentHunger += amount;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
        UpdateHungerBar();

        // Immediately cancel fading out and make the bar visible when feeding
        StopAllCoroutines();
        hungerCanvasGroup.alpha = 1;
        isFading = false;
    }
}
