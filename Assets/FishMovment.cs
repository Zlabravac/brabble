using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public float speed = 2f; // Fish swimming speed

    [Range(0.5f, 3f)]
    public float pauseDuration = 1f; // Slider for pause duration in the Unity Inspector

    public float directionChangeInterval = 2f; // How often the fish changes direction
    public Vector2 movementBounds = new Vector2(8, 4); // X and Y screen bounds

    private Vector2 targetDirection; // Direction the fish is moving
    private SpriteRenderer spriteRenderer; // Reference to the fish's SpriteRenderer
    private bool isPaused = false; // Whether the fish is currently paused

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ChooseRandomDirection();
        InvokeRepeating(nameof(ChooseRandomDirection), directionChangeInterval, directionChangeInterval);
    }

    void Update()
    {
        if (isPaused) return; // Skip movement if paused

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
}
