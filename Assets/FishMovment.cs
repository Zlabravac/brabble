using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 3f; // Speed of the fish
    public float attractionRadius = 5f; // Radius for detecting food
    public float stopDuration = 2f; // How long the fish stops
    public float stopInterval = 5f; // Consistent time between stops

    [Header("References")]
    public HungerBarWithSlider hungerBar; // Reference to the hunger bar
    public Transform targetFood; // Current food target
    public SpriteRenderer spriteRenderer;
    public Animator animator; // Reference to the Animator
    public MoneyManager moneyManager; // Reference to MoneyManager
    public int moneyReward = 10; // Money given when hunger is full

    private Vector2 direction; // Current movement direction
    private bool isStopped = false; // Whether the fish is stopped
    private float stopTimer = 0f; // Timer for stopping
    private float intervalTimer = 0f; // Timer for interval between stops
    private float fishHalfWidth; // Half the width of the fish sprite
    private float fishHalfHeight; // Half the height of the fish sprite

    void Start()
    {
        // Initialize direction and sprite references
        direction = GetRandomDirection();
        spriteRenderer = GetComponent<SpriteRenderer>();
        hungerBar = GetComponent<HungerBarWithSlider>();
        animator = GetComponent<Animator>();
        intervalTimer = stopInterval;

        // Calculate fish dimensions for boundary enforcement
        if (spriteRenderer != null)
        {
            fishHalfWidth = spriteRenderer.bounds.extents.x;
            fishHalfHeight = spriteRenderer.bounds.extents.y;
        }

        moneyManager = FindObjectOfType<MoneyManager>();
    }

    void Update()
    {
        // Reward the player if hunger is full
        if (hungerBar.IsHungerFull())
        {
            if (moneyManager != null)
            {
                moneyManager.AddMoney(moneyReward);
            }
            hungerBar.ModifyHunger(-hungerBar.maxHunger); // Reset hunger
        }

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
            HandleStopInterval();
            animator.SetBool("isMoving", true); // Play animation
        }

        if (targetFood == null)
        {
            FindNearestFood();
        }

        EnforceBoundaries(); // Ensure the fish stays within bounds
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
            intervalTimer = stopInterval;
        }
    }

    private void Wander()
    {
        transform.Translate(direction * speed * Time.deltaTime);
        UpdateSpriteFlip();
    }

    private void MoveTowardsFood()
    {
        if (targetFood == null) return;

        // Move towards the food
        Vector2 foodPosition = targetFood.position;
        direction = (foodPosition - (Vector2)transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime);

        UpdateSpriteFlip();
    }

    private void FindNearestFood()
    {
        Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(transform.position, attractionRadius);
        Transform nearestFood = null;
        float shortestDistance = float.MaxValue;

        foreach (Collider2D obj in nearbyObjects)
        {
            if (obj.CompareTag("Food"))
            {
                Food food = obj.GetComponent<Food>();
                if (food != null)
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

        targetFood = nearestFood;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Boundary"))
        {
            Vector2 collisionNormal = collision.transform.position - transform.position;
            direction = Vector2.Reflect(direction, collisionNormal).normalized;
        }
    }

    private void StopFish()
    {
        isStopped = true;
        stopTimer = stopDuration;
    }

    private void ResumeMovement()
    {
        isStopped = false;
        direction = GetRandomDirection();
    }

    private void UpdateSpriteFlip()
    {
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
        Camera mainCamera = Camera.main;
        float screenHeight = mainCamera.orthographicSize;
        float screenWidth = screenHeight * mainCamera.aspect;

        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -screenWidth + fishHalfWidth, screenWidth - fishHalfWidth);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -screenHeight + fishHalfHeight, screenHeight - fishHalfHeight);

        transform.position = clampedPosition;
    }
}
