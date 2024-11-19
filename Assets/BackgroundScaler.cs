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
        float screenHeight = mainCamera.orthographicSize * 2;
        float screenWidth = screenHeight * Screen.width / Screen.height;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer found on the Background GameObject.");
            return;
        }

        Vector2 spriteSize = spriteRenderer.sprite.bounds.size;

        float scaleX = screenWidth / spriteSize.x;
        float scaleY = screenHeight / spriteSize.y;

        transform.localScale = new Vector3(scaleX, scaleY, 1);
    }
}
