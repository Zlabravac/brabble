using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class DynamicRescaler : MonoBehaviour
{
    public Camera targetCamera;

    void Start()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
        }

        AlignCanvasWithCamera();
    }

    void AlignCanvasWithCamera()
    {
        Canvas canvas = GetComponent<Canvas>();
        RectTransform rectTransform = GetComponent<RectTransform>();

        if (canvas.renderMode == RenderMode.WorldSpace)
        {
            // Camera dimensions
            float cameraHeight = 2f * targetCamera.orthographicSize;
            float cameraWidth = cameraHeight * targetCamera.aspect;

            // Set Canvas size and position
            rectTransform.sizeDelta = new Vector2(cameraWidth, cameraHeight);
            rectTransform.position = new Vector3(targetCamera.transform.position.x, targetCamera.transform.position.y, rectTransform.position.z);

            Debug.Log($"[DynamicRescaler] Canvas aligned: Width={cameraWidth}, Height={cameraHeight}");
        }
        else
        {
            Debug.LogWarning("DynamicRescaler works only with World Space canvases.");
        }
    }
}
