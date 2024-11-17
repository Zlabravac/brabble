using UnityEngine;

public class Food : MonoBehaviour
{
    [Header("Food Settings")]
    public float foodValue = 15f; // Amount of hunger this food restores
    public float decayTime = 7f; // Time before food disappears
    public float fallSpeed = 1f; // Speed at which the food moves downward
    public int moneyValue = 1; // Money gained when this food is eaten

    private float timer = 0f; // Internal timer to track decay
    private bool hasStopped = false; // Tracks if the food has stopped at the bottom

    void Update()
    {
        // Move the food downward unless it has stopped
        if (!hasStopped)
        {
            transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);
        }

        // Increment the decay timer
        timer += Time.deltaTime;

        // Check if the food should decay
        if (timer >= decayTime)
        {
            Destroy(gameObject); // Destroy the food
        }

        // Check if the food has hit the bottom boundary
        CheckBoundaries();
    }

    private void CheckBoundaries()
    {
        Camera mainCamera = Camera.main;
        float screenHeight = mainCamera.orthographicSize;

        // Stop the food if it hits the bottom of the screen
        if (transform.position.y <= -screenHeight)
        {
            hasStopped = true; // Stop the food from moving
            transform.position = new Vector2(transform.position.x, -screenHeight); // Snap to the bottom boundary
        }
    }
}
