using UnityEngine;

public class Food : MonoBehaviour
{
    [Header("Food Settings")]
    public float hungerValue = 20f; // Hunger restored by this food
    public float decayTime = 7f; // Time before food disappears
    public float fallSpeed = 1f; // Speed at which the food moves downward

    [Header("Boundary Reference")]
    public DinamicBounderies dinamicBounderies; // Reference to the fish's boundaries

    private float timer = 0f; // Internal timer to track decay

    void Start()
    {
        // Find the DinamicBounderies script automatically if not assigned
        if (dinamicBounderies == null)
        {
            dinamicBounderies = FindObjectOfType<DinamicBounderies>();
        }
    }

    void Update()
    {
        // Move the food downward
        transform.Translate(Vector2.down * fallSpeed * Time.deltaTime);

        // Clamp the position within the boundaries
        ClampPosition();

        // Increment the decay timer
        timer += Time.deltaTime;

        // Destroy the food if it decays
        if (timer >= decayTime)
        {
            Destroy(gameObject);
        }
    }

    private void ClampPosition()
    {
        if (dinamicBounderies == null)
        {
            Debug.LogWarning("DinamicBounderies reference is missing!");
            return;
        }

        // Get boundaries from the DinamicBounderies script
        float boundaryWidth = dinamicBounderies.boundaryWidth / 2f;
        float boundaryHeight = dinamicBounderies.boundaryHeight / 2f;

        // Clamp the food's position to stay within the boundaries
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -boundaryWidth, boundaryWidth);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, -boundaryHeight, boundaryHeight);
        transform.position = clampedPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Fish"))
        {
            // Feed the fish
            HungerBarWithSlider hungerBar = collision.GetComponent<HungerBarWithSlider>();
            if (hungerBar != null)
            {
                hungerBar.ModifyHunger(hungerValue);
            }

            // Destroy the food
            Destroy(gameObject);
        }
    }
}
