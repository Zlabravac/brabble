using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    public Tilemap placementTilemap;       // Reference to the placement Tilemap
    public GameObject objectToPlacePrefab; // Prefab for objects to place
    public int objectCost = 50;            // Cost of placing an object

    private MoneyManager moneyManager;     // Reference to MoneyManager

    void Start()
    {
        // Find MoneyManager in the scene
        moneyManager = FindObjectOfType<MoneyManager>();
        if (moneyManager == null)
        {
            Debug.LogError("MoneyManager not found in the scene!");
        }

        if (placementTilemap == null)
        {
            Debug.LogError("Placement Tilemap is not assigned!");
        }
    }

    void Update()
    {
        // Detect click or tap for placement
        if (Input.GetMouseButtonDown(0)) // Left-click or touch
        {
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = 0; // Ensure it's on the correct Z-plane
            TryPlaceObject(worldPosition);
        }
    }

    public void TryPlaceObject(Vector3 worldPosition)
    {
        Debug.Log("Attempting to place an object...");

        if (moneyManager == null)
        {
            Debug.LogError("MoneyManager is not assigned or found!");
            return;
        }

        // Convert world position to cell position
        Vector3Int cellPosition = placementTilemap.WorldToCell(worldPosition);
        Debug.Log($"Converted World Position {worldPosition} to Cell Position {cellPosition}");

        // Check if the cell is valid for placement
        if (placementTilemap.HasTile(cellPosition))
        {
            Debug.Log("Valid tile found. Proceeding with placement...");

            // Check if enough money is available
            if (moneyManager.SpendMoney(objectCost))
            {
                // Convert cell position back to world position for object placement
                Vector3 objectPosition = placementTilemap.GetCellCenterWorld(cellPosition);

                Instantiate(objectToPlacePrefab, objectPosition, Quaternion.identity);
                Debug.Log($"Object placed successfully at {cellPosition} for {objectCost} money.");
            }
            else
            {
                Debug.Log("Not enough money to place the object!");
            }
        }
        else
        {
            Debug.Log("Invalid placement: No valid tile in this cell.");
        }
    }
}
