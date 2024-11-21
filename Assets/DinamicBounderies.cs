using UnityEngine;

public class DinamicBounderies : MonoBehaviour
{
    [Header("Boundary Dimensions")]
    public float boundaryWidth = 10f; // Total width of the boundary area
    public float boundaryHeight = 10f; // Total height of the boundary area

    private BoxCollider2D topBoundary;
    private BoxCollider2D bottomBoundary;
    private BoxCollider2D leftBoundary;
    private BoxCollider2D rightBoundary;

    void Start()
    {
        // Create and position boundaries
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
        boundary.transform.parent = transform; // Attach boundary to this object
        BoxCollider2D collider = boundary.AddComponent<BoxCollider2D>();
        collider.isTrigger = true; // Set as trigger to avoid physical collisions
        boundary.tag = "Boundary"; // Tag boundaries for reference
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
        // Draw a rectangle in the Scene view to visualize the boundaries
        Gizmos.color = Color.red;

        float halfWidth = boundaryWidth / 2f;
        float halfHeight = boundaryHeight / 2f;

        // Corners of the boundary
        Vector3 topLeft = transform.position + new Vector3(-halfWidth, halfHeight, 0);
        Vector3 topRight = transform.position + new Vector3(halfWidth, halfHeight, 0);
        Vector3 bottomLeft = transform.position + new Vector3(-halfWidth, -halfHeight, 0);
        Vector3 bottomRight = transform.position + new Vector3(halfWidth, -halfHeight, 0);

        // Draw lines between corners
        Gizmos.DrawLine(topLeft, topRight);
        Gizmos.DrawLine(topRight, bottomRight);
        Gizmos.DrawLine(bottomRight, bottomLeft);
        Gizmos.DrawLine(bottomLeft, topLeft);
    }
}
