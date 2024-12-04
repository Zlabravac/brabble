using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    [Header("Bubble Settings")]
    public GameObject bubblePrefab; // The bubble prefab
    public float bubblesPerMinute = 30f; // Number of bubbles to spawn per minute
    public float bubbleSpeed = 2f; // Speed of the bubbles

    private float spawnInterval; // Time interval between bubble spawns
    private float spawnTimer; // Timer to track bubble spawning

    void Start()
    {
        // Calculate the spawn interval based on bubbles per minute
        spawnInterval = 60f / bubblesPerMinute;
    }

    void Update()
    {
        spawnTimer += Time.deltaTime;

        // Check if it's time to spawn a bubble
        if (spawnTimer >= spawnInterval)
        {
            SpawnBubble();
            spawnTimer = 0f;
        }
    }

    private void SpawnBubble()
    {
        // Get the bottom of the camera view
        Camera mainCamera = Camera.main;
        float bottomY = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane)).y;

        // Get the horizontal range of the camera view
        float leftX = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane)).x;
        float rightX = mainCamera.ViewportToWorldPoint(new Vector3(1, 0, mainCamera.nearClipPlane)).x;

        // Generate a random X position within the range
        float randomX = Random.Range(leftX, rightX);

        // Spawn the bubble at the random position
        Vector3 spawnPosition = new Vector3(randomX, bottomY, 0);
        GameObject bubble = Instantiate(bubblePrefab, spawnPosition, Quaternion.identity);

        // Set the bubble's speed
        BubbleBehavior bubbleScript = bubble.GetComponent<BubbleBehavior>();
        if (bubbleScript != null)
        {
            bubbleScript.speed = bubbleSpeed;
        }
    }
}
