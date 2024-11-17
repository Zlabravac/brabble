using UnityEngine;

public class FishMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float speed = 3f; // Speed of the fish
    public float stopDuration = 1f; // How long the fish stops
    public float stopFrequency = 5f; // How often the fish stops (lower = more frequent)

    private Vector2 direction; // Current movement direction
    private SpriteRenderer spriteRenderer;
    private float stopTimer = 0f; // Timer to manage stopping
    private bool isStopped = false;

    void Start()
    {
        // Initialize the sprite renderer and random direction
        spriteRenderer = GetComponent<SpriteRenderer>();
        direction = GetRandomDirection();
    }

    void Update()
    {
        if (isStopped)
        {
            // Count down the stop timer
            stopTimer -= Time.deltaTime;
            if (stopTimer <= 0)
            {
                ResumeMovement();
            }
        }
        else
        {
            // Move the fish
            transform.Translate(direction * speed * Time.deltaTime);

            // Randomly decide to stop
            if (Random.Range(0f, 1f) < stopFrequency * Time.deltaTime)
            {
                StopFish();
            }

            // Flip the sprite based on the direction
            UpdateSpriteFlip();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the fish hits a boundary
        if (collision.gameObject.name.Contains("Boundary"))
        {
            Vector2 collisionNormal = collision.transform.position - transform.position;
            direction = Vector2.Reflect(direction, collisionNormal).normalized;
            UpdateSpriteFlip(); // Ensure correct sprite flip after direction change
        }
    }

    private void StopFish()
    {
        isStopped = true;
        stopTimer = stopDuration; // Reset stop timer
    }

    private void ResumeMovement()
    {
        isStopped = false;
        direction = GetRandomDirection(); // Set a new random direction
    }

    private void UpdateSpriteFlip()
    {
        if (direction.x > 0)
        {
            spriteRenderer.flipX = true; // Flip when moving right
        }
        else if (direction.x < 0)
        {
            spriteRenderer.flipX = false; // Default when moving left
        }
    }

    private Vector2 GetRandomDirection()
    {
        // Generate a new random direction
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
