using UnityEngine;

public class SwipeCameraController : MonoBehaviour
{
    public float swipeSpeed = 0.005f; // Adjust for swipe sensitivity
    public float minX; // Minimum X position
    public float maxX; // Maximum X position

    private Vector3 initialTouchPosition;

    void Update()
    {
        HandleSwipe();
    }

    void HandleSwipe()
    {
        if (Input.GetMouseButtonDown(0))
        {
            initialTouchPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 currentTouchPosition = Input.mousePosition;
            float deltaX = (initialTouchPosition.x - currentTouchPosition.x) * swipeSpeed;

            Vector3 newPosition = transform.position;
            newPosition.x = Mathf.Clamp(transform.position.x + deltaX, minX, maxX);
            transform.position = newPosition;

            initialTouchPosition = currentTouchPosition;
        }
    }

    // Method to update the maximum X boundary
    public void UpdateCameraBounds(float newMaxX)
    {
        maxX = newMaxX;
    }
}
