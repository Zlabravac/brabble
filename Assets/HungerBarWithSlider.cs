using UnityEngine;
using UnityEngine.UI;

public class HungerBarWithSlider : MonoBehaviour
{
    [Header("Hunger Settings")]
    public float maxHunger = 100f; // Maximum hunger level
    public float hungerDecreaseRate = 1f; // Hunger decreases per second

    [Header("UI Components")]
    public Slider hungerSlider; // Reference to the Slider UI component
    public Image hungerFillImage; // Fill image for changing colors

    private float currentHunger;

    void Start()
    {
        // Initialize hunger levels
        currentHunger = maxHunger / 2; // Start with half-full hunger bar
        hungerSlider.maxValue = maxHunger;
        hungerSlider.value = currentHunger;
    }

    void Update()
    {
        // Decrease hunger over time
        currentHunger -= hungerDecreaseRate * Time.deltaTime;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger); // Ensure hunger stays within bounds

        // Update the slider and fill color
        UpdateHungerBar();
    }

    public void ModifyHunger(float amount)
    {
        // Increase or decrease hunger (clamped to maxHunger)
        currentHunger += amount;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
        UpdateHungerBar();
    }

    public bool IsHungerFull()
    {
        // Return true if hunger is exactly 100%
        return currentHunger >= maxHunger;
    }

    private void UpdateHungerBar()
    {
        // Update the slider value
        hungerSlider.value = currentHunger;

        // Update the color (green to red)
        float t = currentHunger / maxHunger; // Normalize hunger value
        hungerFillImage.color = Color.Lerp(Color.red, Color.green, t);
    }
}
