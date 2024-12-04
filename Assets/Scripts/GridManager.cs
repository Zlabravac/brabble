using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int gridWidth = 10;  // Number of cells horizontally
    public int gridHeight = 10; // Number of cells vertically
    public float cellSize = 1f; // Size of each cell
    public GameObject gridCellPrefab; // Prefab for the grid cell

    private GameObject[,] gridCells; // Array to store the grid cells

    void Start()
    {
        CreateGrid();
    }

    void CreateGrid()
    {
        gridCells = new GameObject[gridWidth, gridHeight];

        // Get the bottom-left corner of the camera's view in world space
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        bottomLeft.z = 0; // Ensure the Z position is 0 for a 2D game

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                // Calculate the position for each cell
                Vector3 position = bottomLeft + new Vector3(x * cellSize, y * cellSize, 0);

                // Instantiate the grid cell prefab at the calculated position
                GameObject cell = Instantiate(gridCellPrefab, position, Quaternion.identity, transform);

                cell.name = $"Cell_{x}_{y}"; // Name the cell for debugging
                gridCells[x, y] = cell; // Store the cell in the grid array
            }
        }
    }

    // Optional: Get grid position based on world position
    public Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.FloorToInt((worldPosition.x - Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).x) / cellSize);
        int y = Mathf.FloorToInt((worldPosition.y - Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0)).y) / cellSize);
        return new Vector2Int(x, y);
    }

    // Optional: Get world position based on grid position
    public Vector3 GetWorldPosition(Vector2Int gridPosition)
    {
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        bottomLeft.z = 0; // Ensure the Z position is 0
        return bottomLeft + new Vector3(gridPosition.x * cellSize, gridPosition.y * cellSize, 0);
    }
}
