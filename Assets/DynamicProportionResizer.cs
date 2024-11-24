using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class DynamicProportionalResizer : MonoBehaviour
{
    public Vector2 referenceResolution = new Vector2(1080, 1920); // Reference resolution

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        AdjustSize();
    }

    void AdjustSize()
    {
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        Vector2 scaleFactor = new Vector2(
            screenSize.x / referenceResolution.x,
            screenSize.y / referenceResolution.y
        );

        // Maintain proportional scaling
        float finalScale = Mathf.Min(scaleFactor.x, scaleFactor.y);

        // Apply scaling
        rectTransform.localScale = new Vector3(finalScale, finalScale, 1);

        Debug.Log($"[DynamicProportionalResizer] Scaled: {rectTransform.name}, Final Scale: {finalScale}, Screen Size: {screenSize}");
    }
}
