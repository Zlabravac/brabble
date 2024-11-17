using UnityEngine;

public class Food : MonoBehaviour
{
    [Header("Food Settings")]
    public float foodValue = 15f; // Amount of hunger this food restores
    public float decayTime = 7f; // Time before food disappears
    public float fallSpeed = 1f; // Speed at which the food moves downward

    private float timer = 0f; // Internal timer to track decay

    void Update()
    {
        // Move the food downward
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);

        // Increment the decay timer
        timer += Time.deltaTime;

        // Check if the food should decay
        if (timer >= decayTime)
        {
            Destroy(gameObject); // Destroy the food
        }
    }
}
