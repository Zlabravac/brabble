using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class DynamicRescaler : MonoBehaviour
{
    public GameObject background; // The Background GameObject to match

    void Start()
    {
        if (background == null)
        {
            Debug.LogError("Background GameObject is not assigned to DynamicRescaler.");
            return;
        }

        AlignCanvasWithBackground();
    }

    void AlignCanvasWithBackground()
    {
        Canvas canvas = GetComponent<Canvas>();

        if (canvas.renderMode == RenderMode.WorldSpace)
        {
            RectTransform rectTransform = GetComponent<RectTransform>();

            // Get the size and position of the background from its SpriteRenderer
            SpriteRenderer spriteRenderer = background.GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
            {
                Debug.LogError("No SpriteRenderer found on the assigned Background GameObject.");
                return;
            }

            // Get the background size from the SpriteRenderer's bounds
            Vector2 backgroundSize = spriteRenderer.bounds.size;

            // Set the Canvas size to match the background size
            rectTransform.sizeDelta = new Vector2(backgroundSize.x, backgroundSize.y);

            // Align the Canvas position with the Background's position
            rectTransform.position = new Vector3(background.transform.position.x, background.transform.position.y, rectTransform.position.z);

            // Ensure the Canvas rotation matches the default (no rotation)
            rectTransform.rotation = Quaternion.identity;

            Debug.Log($"Canvas aligned with Background: Width={backgroundSize.x}, Height={backgroundSize.y}, Position={rectTransform.position}");
        }
        else
        {
            Debug.LogWarning("This script works only with World Space canvases.");
        }
    }
}
