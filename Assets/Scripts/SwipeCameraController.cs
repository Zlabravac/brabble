using UnityEngine;

public class SwipeCameraController : MonoBehaviour
{
    public float swipeSpeed = 0.005f; // Adjust for swipe sensitivity
    public float minX = 0f; // Minimum X position
    public float maxX = 10f; // Maximum X position

    private Vector3 lastMousePosition; // Tracks the last frame's mouse position
    private bool isDragging = false; // To track if the user is swiping

    void Update()
    {
        HandleSwipe();
    }

    void HandleSwipe()
    {
        // Detect the start of a swipe
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
            isDragging = true;
        }

        // Detect the end of a swipe
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        // Handle the swipe movement
        if (isDragging)
        {
            Vector3 currentMousePosition = Input.mousePosition;
            float deltaX = (lastMousePosition.x - currentMousePosition.x) * swipeSpeed;

            Vector3 newPosition = transform.position;
            newPosition.x = Mathf.Clamp(transform.position.x + deltaX, minX, maxX); // Restrict movement to bounds
            transform.position = newPosition;

            lastMousePosition = currentMousePosition; // Update the last position
        }
    }

    public void UpdateCameraBounds(float newMaxX)
    {
        maxX = newMaxX; // Update the maximum X boundary
    }
}
