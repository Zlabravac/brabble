using UnityEngine;

public class BubbleBehavior : MonoBehaviour
{
    public float speed = 2f; // Speed of the bubble's upward movement

    void Update()
    {
        // Log the current position of the bubble
        Debug.Log($"Bubble Position: {transform.position}");

        // Move the bubble upward
        transform.position += Vector3.up * speed * Time.deltaTime;

        // Check if the bubble is outside the camera view and destroy it
        if (!IsInCameraView())
        {
            Debug.Log("Bubble out of view and destroyed");
            Destroy(gameObject);
        }
    }

    private bool IsInCameraView()
    {
        Camera mainCamera = Camera.main;
        Vector3 bubblePosition = mainCamera.WorldToViewportPoint(transform.position);

        // Check if the bubble is outside the camera view
        return bubblePosition.x >= 0 && bubblePosition.x <= 1 && bubblePosition.y >= 0 && bubblePosition.y <= 1;
    }
}
