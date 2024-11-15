using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public float speed = 2f; // Fish swimming speed
    public float directionChangeInterval = 2f; // How often the fish changes direction
    public Vector2 movementBounds = new Vector2(8, 4); // X and Y screen bounds

    private Vector2 targetDirection; // Direction the fish is moving

    void Start()
    {
        // Set an initial random direction
        ChooseRandomDirection();
        // Change direction periodically
        InvokeRepeating(nameof(ChooseRandomDirection), directionChangeInterval, directionChangeInterval);
    }

    void Update()
    {
        // Move the fish
        transform.Translate(targetDirection * speed * Time.deltaTime);

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
        // Pick a random direction
        targetDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
