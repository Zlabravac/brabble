using UnityEngine;

public class GridCell : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Color defaultColor = Color.white;
    public Color highlightColor = Color.green;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Highlight()
    {
        if (spriteRenderer != null)
            spriteRenderer.color = highlightColor;
    }

    public void ResetColor()
    {
        if (spriteRenderer != null)
            spriteRenderer.color = defaultColor;
    }
}
