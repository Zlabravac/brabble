using UnityEngine;

public class ScaleWithCanvas : MonoBehaviour
{
    private Vector2 initialCanvasSize;
    private Vector3 initialScale;

    void Start()
    {
        // Capture the initial size of the parent Canvas and the UI element's scale
        RectTransform canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        initialCanvasSize = canvasRect.sizeDelta;
        initialScale = transform.localScale;
    }

    void Update()
    {
        // Get the current Canvas size
        RectTransform canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
        Vector2 currentCanvasSize = canvasRect.sizeDelta;

        // Scale the UI element proportionally to the change in Canvas size
        float scaleX = currentCanvasSize.x / initialCanvasSize.x;
        float scaleY = currentCanvasSize.y / initialCanvasSize.y;

        // Apply the new scale
        transform.localScale = new Vector3(initialScale.x * scaleX, initialScale.y * scaleY, initialScale.z);
    }
}
