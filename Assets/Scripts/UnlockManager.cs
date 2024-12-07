using UnityEngine;

public class UnlockManager : MonoBehaviour
{
    public SwipeCameraController cameraController; // Reference to SwipeCameraController
    public float[] unlockPositions; // X positions for each unlockable area
    private int currentUnlockIndex = 0; // Tracks the current unlock stage

    public void UnlockNextArea()
    {
        if (currentUnlockIndex < unlockPositions.Length) // Ensure there are areas to unlock
        {
            float newMaxX = unlockPositions[currentUnlockIndex]; // Get the next unlock position
            cameraController.UpdateCameraBounds(newMaxX); // Update the camera boundary
            currentUnlockIndex++; // Move to the next unlock stage
        }
        else
        {
            Debug.Log("No more areas to unlock!"); // Notify if all areas are unlocked
        }
    }
}
