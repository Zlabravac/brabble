using UnityEngine;

public class DynamicBoundaries : MonoBehaviour
{
    private BoxCollider2D topBoundary;
    private BoxCollider2D bottomBoundary;
    private BoxCollider2D leftBoundary;
    private BoxCollider2D rightBoundary;

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
        boundary.transform.parent = transform;
        BoxCollider2D collider = boundary.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        boundary.tag = "Boundary";
        return collider;
    }

    void AdjustBoundaries()
    {
        Camera mainCamera = Camera.main;

        float screenHeight = mainCamera.orthographicSize;
        float screenWidth = screenHeight * mainCamera.aspect;

        topBoundary.offset = new Vector2(0, screenHeight);
        topBoundary.size = new Vector2(screenWidth * 2, 0.1f);

        bottomBoundary.offset = new Vector2(0, -screenHeight);
        bottomBoundary.size = new Vector2(screenWidth * 2, 0.1f);

        leftBoundary.offset = new Vector2(-screenWidth, 0);
        leftBoundary.size = new Vector2(0.1f, screenHeight * 2);

        rightBoundary.offset = new Vector2(screenWidth, 0);
        rightBoundary.size = new Vector2(0.1f, screenHeight * 2);
    }
}
