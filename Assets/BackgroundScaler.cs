using UnityEngine;

public class BackgroundScaler : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        ScaleToFitCamera();
    }

    void ScaleToFitCamera()
    {
        // Get the dimensions of the camera's view in world units
        float screenHeight = mainCamera.orthographicSize * 2;
        float screenWidth = screenHeight * Screen.width / Screen.height;

        // Get the size of the sprite's bounds
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer found on the Background GameObject.");
            return;
        }

        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        // Calculate the scale to fit the camera
        float scaleX = screenWidth / spriteSize.x;
        float scaleY = screenHeight / spriteSize.y;

        // Apply the scale
        transform.localScale = new Vector3(scaleX, scaleY, 1);
    }
}
