using UnityEngine;
using UnityEngine.Tilemaps; // Required for Tilemap functionality

public class GridPlacement : MonoBehaviour
{
    public GameObject placeablePrefab; // Drag your prefab here
    public Tilemap tilemap; // Assign your Tilemap in the Inspector

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detect left mouse click or touch
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int gridPosition = tilemap.WorldToCell(worldPoint);

            if (tilemap.HasTile(gridPosition)) // Check if clicked cell has a tile
            {
                PlaceObject(gridPosition);
            }
        }
    }

    void PlaceObject(Vector3Int gridPosition)
    {
        Vector3 placePosition = tilemap.GetCellCenterWorld(gridPosition); // Get world position of the grid cell
        Instantiate(placeablePrefab, placePosition, Quaternion.identity); // Place the object
    }
}
