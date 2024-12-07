using UnityEngine;

public class UnlockArea : MonoBehaviour
{
    public GameObject newSection; // Reference to the locked Tilemap (e.g., Tilemap_LockedSection)
    public CameraController cameraController; // Reference to the CameraController script
    public Vector2 newMinBoundary; // New minimum boundary for the expanded area
    public Vector2 newMaxBoundary; // New maximum boundary for the expanded area

    public GameObject unlockButton; // Reference to the unlock button itself

    public void UnlockNewArea()
    {
        // Activate the new section
        newSection.SetActive(true);

        // Merge current and new boundaries to avoid conflicts
        MergeBoundaries();

        // Disable the unlock button to prevent re-clicking
        unlockButton.SetActive(false);

        Debug.Log("New area unlocked!");
    }

    private void MergeBoundaries()
    {
        // Adjust the camera boundaries by merging the current ones with the new ones
        cameraController.minBoundary = new Vector2(
            Mathf.Min(cameraController.minBoundary.x, newMinBoundary.x),
            Mathf.Min(cameraController.minBoundary.y, newMinBoundary.y)
        );

        cameraController.maxBoundary = new Vector2(
            Mathf.Max(cameraController.maxBoundary.x, newMaxBoundary.x),
            Mathf.Max(cameraController.maxBoundary.y, newMaxBoundary.y)
        );

        Debug.Log($"Updated camera boundaries: Min = {cameraController.minBoundary}, Max = {cameraController.maxBoundary}");
    }

    private void OnDrawGizmos()
    {
        // Visualize the new boundaries in the Scene view
        Gizmos.color = Color.green;

        Vector3 bottomLeft = new Vector3(newMinBoundary.x, newMinBoundary.y, 0);
        Vector3 bottomRight = new Vector3(newMaxBoundary.x, newMinBoundary.y, 0);
        Vector3 topLeft = new Vector3(newMinBoundary.x, newMaxBoundary.y, 0);
        Vector3 topRight = new Vector3(newMaxBoundary.x, newMaxBoundary.y, 0);

        // Draw the rectangle for the new boundaries
        Gizmos.DrawLine(bottomLeft, bottomRight);
        Gizmos.DrawLine(bottomRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, bottomLeft);
    }
}
