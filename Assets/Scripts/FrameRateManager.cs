using UnityEngine;

public class FrameRateManager : MonoBehaviour
{
    void Start()
    {
        SetMaxFrameRate();
    }

    void SetMaxFrameRate()
    {
        // Get the current refresh rate of the screen
        int refreshRate = Screen.currentResolution.refreshRate;

        if (refreshRate > 0)
        {
            // Set the target frame rate to the refresh rate
            Application.targetFrameRate = refreshRate;
            Debug.Log("Setting frame rate to match screen refresh rate: " + refreshRate + " Hz");
        }
        else
        {
            // Default to 60 FPS if refresh rate cannot be determined
            Application.targetFrameRate = 60;
            Debug.Log("Unable to determine refresh rate, setting frame rate to default: 60 FPS");
        }
    }
}
