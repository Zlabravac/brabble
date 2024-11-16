using UnityEngine;
using UnityEngine.UI;

public class FishMovement : MonoBehaviour
{
    public float speed = 2f; // Fish swimming speed
    public float directionChangeInterval = 2f; // How often the fish changes direction
    [Range(0.5f, 3f)]
    public float pauseDuration = 1f; // Slider for pause duration in the Unity Inspector
    public Vector2 movementBounds = new Vector2(8, 4); // X and Y screen bounds

    public Slider hungerBar; // Reference to the hunger bar
    public Image hungerBarFill; // Reference to the Fill Area of the slider
    public float hungerDecreaseRate = 10f; // Adjustable rate of hunger decrease per second
    public float maxHunger = 100f; // Maximum hunger level

    public Color fullHungerColor = Color.green; // Color for full hunger
    public Color midHungerColor = Color.yellow; // Color for medium hunger
    public Color lowHungerColor = Color.red; // Color for low hunger

    [Range(0f, 0.1f)]
    public float minimumFillScale = 0.05f; // Minimum visible fill when slider is at 0

    private Vector2 targetDirection; // Direction the fish is moving
    private SpriteRenderer spriteRenderer; // Reference to the fish's SpriteRenderer
    private bool isPaused = false; // Whether the fish is currently paused
    private float currentHunger; // Current hunger level

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHunger = maxHunger; // Initialize hunger to the maximum level
        hungerBar.maxValue = maxHunger; // Set the slider max value
        hungerBar.value = maxHunger; // Initialize slider value
        ChooseRandomDirection();
        InvokeRepeating(nameof(ChooseRandomDirection), directionChangeInterval, directionChangeInterval);
    }

    void Update()
    {
        // Constant hunger decrease over time
        if (currentHunger > 0)
        {
            currentHunger -= hungerDecreaseRate * Time.deltaTime; // Decrease hunger at the set rate
            currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger); // Ensure it doesn't go below 0
            hungerBar.value = currentHunger; // Update the hunger bar
            UpdateHungerBarColor(); // Update the color of the hunger bar
            UpdateHungerBarFill(); // Adjust fill visibility
        }

        // Skip movement if paused
        if (isPaused) return;

        // Move the fish
        transform.Translate(targetDirection * speed * Time.deltaTime);

        // Flip the sprite if the fish is moving left
        if (targetDirection.x < 0)
            spriteRenderer.flipX = true; // Flip the sprite
        else if (targetDirection.x > 0)
            spriteRenderer.flipX = false; // Reset the flip

        // Check if the fish is out of bounds and adjust its direction
        if (Mathf.Abs(transform.position.x) > movementBounds.x)
        {
            transform.position = new Vector2(Mathf.Sign(transform.position.x) * movementBounds.x, transform.position.y);
            targetDirection.x *= -1; // Reverse direction on X
        }

        if (Mathf.Abs(transform.position.y) > movementBounds.y)
        {
            transform.position = new Vector2(transform.position.x, Mathf.Sign(transform.position.y) * movementBounds.y);
            targetDirection.y *= -1; // Reverse direction on Y
        }
    }

    void ChooseRandomDirection()
    {
        StartCoroutine(PauseBeforeMoving());
    }

    System.Collections.IEnumerator PauseBeforeMoving()
    {
        isPaused = true; // Stop movement
        yield return new WaitForSeconds(pauseDuration); // Wait for the specified duration
        isPaused = false; // Resume movement
        targetDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    public void RefillHunger(float amount)
    {
        currentHunger = Mathf.Clamp(currentHunger + amount, 0, maxHunger); // Clamp between 0 and max hunger
        hungerBar.value = currentHunger; // Update the hunger bar
        UpdateHungerBarColor(); // Update the color
        UpdateHungerBarFill(); // Adjust fill visibility
    }

    private void UpdateHungerBarColor()
    {
        // Calculate the percentage of hunger remaining
        float hungerPercentage = currentHunger / maxHunger;

        // Determine the color based on the percentage
        if (hungerPercentage > 0.5f)
        {
            // Blend from green to yellow
            hungerBarFill.color = Color.Lerp(midHungerColor, fullHungerColor, (hungerPercentage - 0.5f) * 2);
        }
        else
        {
            // Blend from yellow to red
            hungerBarFill.color = Color.Lerp(lowHungerColor, midHungerColor, hungerPercentage * 2);
        }
    }

    private void UpdateHungerBarFill()
    {
        // Adjust fill amount to include minimum visible fill
        float hungerPercentage = currentHunger / maxHunger;

        // Adjust fill amount dynamically
        hungerBarFill.fillAmount = Mathf.Max(hungerPercentage, minimumFillScale);
    }
}
