using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundScaler : MonoBehaviour
{
    public Camera targetCamera; // Reference to the camera

    void Start()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main; // Use Main Camera if none is assigned
        }

        ScaleBackground();
    }

    void ScaleBackground()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer found on this GameObject!");
            return;
        }

        // Get the camera's dimensions
        float cameraHeight = 2f * targetCamera.orthographicSize;
        float cameraWidth = cameraHeight * targetCamera.aspect;

        // Get the background sprite's dimensions
        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        // Calculate the scaling factors
        float scaleX = cameraWidth / spriteSize.x;
        float scaleY = cameraHeight / spriteSize.y;

        // Apply the larger scaling factor to ensure full coverage
        float finalScale = Mathf.Max(scaleX, scaleY);

        transform.localScale = new Vector3(finalScale, finalScale, 1);

        // Center the background
        transform.position = new Vector3(targetCamera.transform.position.x, targetCamera.transform.position.y, transform.position.z);

        Debug.Log($"[BackgroundScaler] Background scaled to: FinalScale={finalScale}, CameraWidth={cameraWidth}, CameraHeight={cameraHeight}");
    }
}
