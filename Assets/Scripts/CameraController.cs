using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the camera movement
    public float maxX; // The maximum X-axis position the camera can move to

    private Vector3 targetPosition;

    void Start()
    {
        // Initialize the target position to the current position
        targetPosition = transform.position;
    }

    void Update()
    {
        // Smoothly move the camera along the X-axis only
        transform.position = Vector3.Lerp(
            transform.position,
            new Vector3(targetPosition.x, transform.position.y, transform.position.z),
            moveSpeed * Time.deltaTime
        );
    }

    public void UnlockNewArea(float newMaxX)
    {
        maxX = newMaxX; // Update the max X position
    }

    public void MoveToUnlockedArea()
    {
        // Update the target X position while preserving Y and Z
        targetPosition = new Vector3(maxX, transform.position.y, transform.position.z);
    }
}
