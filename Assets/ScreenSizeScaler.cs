using UnityEngine;

[ExecuteAlways]
public class AutoScreenScaler : MonoBehaviour
{
    private Vector3 originalScale; // Store the original scale of the object

    void Start()
    {
        // Store the object's original scale for reference
        originalScale = transform.localScale;

        // Automatically scale the object
        ScaleToScreen();
    }

    void ScaleToScreen()
    {
        // Get the height of the screen in world units (camera's orthographic view)
        float screenHeightInWorldUnits = Camera.main.orthographicSize * 2;

        // Get the width of the screen in world units
        float screenWidthInWorldUnits = screenHeightInWorldUnits * Camera.main.aspect;

        // Determine the relative size of the object based on the screen height
        float scaleFactor = screenHeightInWorldUnits / 1920f; // Assuming 1920 is your base height in pixels

        // Apply the scale factor to the object's original scale
        transform.localScale = originalScale * scaleFactor;

        // Debugging: Log scaling information
        Debug.Log($"[AutoScreenScaler] Screen Height: {screenHeightInWorldUnits}, Screen Width: {screenWidthInWorldUnits}, Scale Factor: {scaleFactor}, New Scale: {transform.localScale}");
    }

    void Update()
    {
#if UNITY_EDITOR
        // Continuously scale in the editor for testing
        ScaleToScreen();
#endif
    }
}
