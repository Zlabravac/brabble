using UnityEngine;

public class SectionUnlocker : MonoBehaviour
{
    public CameraController cameraController; // Reference to the CameraController script
    public Vector2 newMinBoundary; // New minimum boundary after unlocking
    public Vector2 newMaxBoundary; // New maximum boundary after unlocking
    public GameObject unlockButton; // The button itself
    public GameObject newSection; // The new section to reveal
    public int unlockCost = 100; // Cost to unlock the section

    public void UnlockSection()
    {
        if (HasEnoughMoney(unlockCost))
        {
            DeductMoney(unlockCost);

            // Expand the camera boundaries
            cameraController.minBoundary = newMinBoundary;
            cameraController.maxBoundary = newMaxBoundary;

            // Reveal the new section
            newSection.SetActive(true);

            // Disable the unlock button
            unlockButton.SetActive(false);
        }
    }

    private bool HasEnoughMoney(int cost)
    {
        // Replace with your actual money-checking logic
        return true;
    }

    private void DeductMoney(int amount)
    {
        // Replace with your actual money-deducting logic
    }
}
