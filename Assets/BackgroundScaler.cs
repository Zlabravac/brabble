using UnityEngine;

public class BackgroundScaler : MonoBehaviour
{
    void Start()
    {
        ScaleBackground();
    }

    void ScaleBackground()
    {
        // Get the SpriteRenderer attached to the background
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        // Get the background's size in world units
        Vector3 spriteSize = sr.bounds.size;

        // Get the screen size in world units based on the camera's orthographic size
        float screenHeight = Camera.main.orthographicSize * 2;
        float screenWidth = screenHeight * Screen.width / Screen.height;

        // Calculate the scale factor
        Vector3 scale = transform.localScale;
        scale.x = screenWidth / spriteSize.x;
        scale.y = screenHeight / spriteSize.y;

        // Apply the scale
        transform.localScale = scale;
    }
}
