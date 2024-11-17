using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 3f; // Speed of the fish
    public float attractionRadius = 5f; // Radius for detecting food
    public float stopDuration = 2f; // How long the fish stops
    public float stopInterval = 5f; // Consistent time between stops

    private HungerBarWithSlider hungerBar; // Reference to the hunger bar
    private Transform targetFood; // Current food target
    private Vector2 direction; // Current movement direction
    private SpriteRenderer spriteRenderer;
    private bool isStopped = false; // Whether the fish is stopped
    private float stopTimer = 0f; // Timer for stopping
    private float intervalTimer = 0f; // Timer for interval between stops
    private Animator animator; // Reference to the Animator
    private float fishHalfWidth; // Half the width of the fish sprite
    private float fishHalfHeight; // Half the height of the fish sprite

    void Start()
    {
        direction = GetRandomDirection(); // Start with a random direction
        spriteRenderer = GetComponent<SpriteRenderer>();
        hungerBar = GetComponent<HungerBarWithSlider>(); // Reference to hunger bar
        intervalTimer = stopInterval; // Initialize the interval timer
        animator = GetComponent<Animator>(); // Get Animator component

        // Calculate half of the sprite's dimensions
        if (spriteRenderer != null)
        {
            fishHalfWidth = spriteRenderer.bounds.extents.x;
            fishHalfHeight = spriteRenderer.bounds.extents.y;
        }
    }

    void Update()
    {
        if (isStopped)
        {
            HandleStopping();
            animator.SetBool("isMoving", false); // Stop animation
        }
        else if (targetFood != null)
        {
            MoveTowardsFood();
            animator.SetBool("isMoving", true); // Play animation
        }
        else
        {
            Wander();
            HandleStopInterval(); // Manage the consistent stopping logic
            animator.SetBool("isMoving", true); // Play animation
        }

        if (targetFood == null)
        {
            FindNearestFood();
        }

        // Enforce boundaries to prevent fish from escaping
        EnforceBoundaries();
    }

    private void HandleStopping()
    {
        stopTimer -= Time.deltaTime;
        if (stopTimer <= 0)
        {
            ResumeMovement();
        }
    }

    private void HandleStopInterval()
    {
        intervalTimer -= Time.deltaTime;
        if (intervalTimer <= 0)
        {
            StopFish();
            intervalTimer = stopInterval; // Reset the interval timer
        }
    }

    private void Wander()
    {
        // Continue moving in the current direction
        transform.Translate(direction * speed * Time.deltaTime);

        // Flip the sprite based on movement direction
        UpdateSpriteFlip();
    }

    private void MoveTowardsFood()
    {
        if (targetFood == null) return;

        // Move towards the food
        Vector2 foodPosition = targetFood.position;

        // Clamp the food position to stay within boundaries
        Camera mainCamera = Camera.main;
        float screenHeight = mainCamera.orthographicSize;
        float screenWidth = screenHeight * mainCamera.aspect;

        foodPosition.x = Mathf.Clamp(foodPosition.x, -screenWidth, screenWidth);
        foodPosition.y = Mathf.Clamp(foodPosition.y, -screenHeight, screenHeight);

        direction = (foodPosition - (Vector2)transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime);
        UpdateSpriteFlip();
    }

    private void FindNearestFood()
    {
        // Look for the nearest food within the attraction radius
        Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(transform.position, attractionRadius);
        Transform nearestFood = null;
        float shortestDistance = float.MaxValue;

        foreach (Collider2D obj in nearbyObjects)
        {
            if (obj.CompareTag("Food"))
            {
                Food food = obj.GetComponent<Food>();
                if (food != null && CanEatFood(food.foodValue))
                {
                    float distance = Vector2.Distance(transform.position, obj.transform.position);
                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        nearestFood = obj.transform;
                    }
                }
            }
        }

        targetFood = nearestFood; // Set the nearest eligible food as the target
    }

    private bool CanEatFood(float foodValue)
    {
        // Check if the food value + current hunger does not exceed max hunger
        float currentHunger = hungerBar.GetCurrentHunger();
        float maxHunger = hungerBar.GetMaxHunger();
        return (currentHunger + foodValue) <= maxHunger;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boundary"))
        {
            // Reflect direction to stay within boundaries
            Vector2 collisionNormal = collision.transform.position - transform.position;
            direction = Vector2.Reflect(direction, collisionNormal).normalized;
        }
        else if (collision.CompareTag("Food"))
        {
            Food food = collision.GetComponent<Food>();
            if (food != null && CanEatFood(food.foodValue))
            {
                // Consume the food
                Destroy(collision.gameObject);
                IncreaseHunger(food.foodValue); // Increase hunger by the food's value

                // Reset the target food
                targetFood = null;

                // Check for more food if still hungry
                if (hungerBar.GetCurrentHunger() < hungerBar.GetMaxHunger())
                {
                    FindNearestFood();
                }
            }
        }
    }

    private void StopFish()
    {
        isStopped = true;
        stopTimer = stopDuration; // Set the stop timer
    }

    private void ResumeMovement()
    {
        isStopped = false;
        direction = GetRandomDirection(); // Set a new random direction
    }

    private void IncreaseHunger(float amount)
    {
        if (hungerBar != null)
        {
            hungerBar.ModifyHunger(amount);
        }
    }

    private void UpdateSpriteFlip()
    {
        // Flip the sprite based on direction
        if (direction.x > 0)
            spriteRenderer.flipX = true; // Moving right
        else if (direction.x < 0)
            spriteRenderer.flipX = false; // Moving left
    }

    private Vector2 GetRandomDirection()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    private void EnforceBoundaries()
    {
        // Get the boundaries of the camera
        Camera mainCamera = Camera.main;
        float screenHeight = mainCamera.orthographicSize;
        float screenWidth = screenHeight * mainCamera.aspect;

        // Clamp the fish's position within the screen bounds, considering the sprite's size
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -screenWidth + fishHalfWidth, screenWidth - fishHalfWidth);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -screenHeight + fishHalfHeight, screenHeight - fishHalfHeight);

        transform.position = clampedPosition;
    }
}
