using UnityEngine;

public class DynamicBoundaries : MonoBehaviour
{
    private BoxCollider2D topBoundary;
    private BoxCollider2D bottomBoundary;
    private BoxCollider2D leftBoundary;
    private BoxCollider2D rightBoundary;

    [Header("Custom Boundary Settings")]
    public float boundaryWidth = 5f; // Custom width of the boundary
    public float boundaryHeight = 5f; // Custom height of the boundary
    public Color boundaryColor = Color.red; // Color of the boundary lines

    void Start()
    {
        CreateBoundaries();
        AdjustBoundaries();
    }

    void CreateBoundaries()
    {
        topBoundary = CreateBoundary("TopBoundary");
        bottomBoundary = CreateBoundary("BottomBoundary");
        leftBoundary = CreateBoundary("LeftBoundary");
        rightBoundary = CreateBoundary("RightBoundary");
    }

    BoxCollider2D CreateBoundary(string name)
    {
        GameObject boundary = new GameObject(name);
        boundary.transform.parent = transform; // Attach to the parent GameObject
        BoxCollider2D collider = boundary.AddComponent<BoxCollider2D>();
        collider.isTrigger = true; // Set as trigger to detect overlaps without physical interaction
        boundary.tag = "Boundary"; // Tag the boundary for detection
        return collider;
    }

    void AdjustBoundaries()
    {
        // Calculate half dimensions
        float halfWidth = boundaryWidth / 2f;
        float halfHeight = boundaryHeight / 2f;

        // Position and size the boundaries
        topBoundary.offset = new Vector2(0, halfHeight);
        topBoundary.size = new Vector2(boundaryWidth, 0.1f);

        bottomBoundary.offset = new Vector2(0, -halfHeight);
        bottomBoundary.size = new Vector2(boundaryWidth, 0.1f);

        leftBoundary.offset = new Vector2(-halfWidth, 0);
        leftBoundary.size = new Vector2(0.1f, boundaryHeight);

        rightBoundary.offset = new Vector2(halfWidth, 0);
        rightBoundary.size = new Vector2(0.1f, boundaryHeight);
    }

    void OnDrawGizmos()
    {
        // Only draw boundaries if the script is active in the scene
        Gizmos.color = boundaryColor;

        // Calculate half dimensions
        float halfWidth = boundaryWidth / 2f;
        float halfHeight = boundaryHeight / 2f;

        // Draw a rectangle representing the boundaries
        Vector3 topLeft = transform.position + new Vector3(-halfWidth, halfHeight, 0);
        Vector3 topRight = transform.position + new Vector3(halfWidth, halfHeight, 0);
        Vector3 bottomLeft = transform.position + new Vector3(-halfWidth, -halfHeight, 0);
        Vector3 bottomRight = transform.position + new Vector3(halfWidth, -halfHeight, 0);

        Gizmos.DrawLine(topLeft, topRight); // Top edge
        Gizmos.DrawLine(topRight, bottomRight); // Right edge
        Gizmos.DrawLine(bottomRight, bottomLeft); // Bottom edge
        Gizmos.DrawLine(bottomLeft, topLeft); // Left edge
    }
}
