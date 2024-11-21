using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject foodPrefab; // The food prefab
    public Camera mainCamera; // Reference to the camera
    public DynamicBoundaries boundaries; // Reference to the DynamicBoundaries script

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click
        {
            SpawnFood();
        }
    }

    void SpawnFood()
    {
        // Convert mouse position to world position
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 10f; // Set Z distance from camera
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

        // Check if the click is within boundaries
        if (IsWithinBoundaries(worldPosition))
        {
            // Spawn the food prefab at the clicked position
            Instantiate(foodPrefab, worldPosition, Quaternion.identity);
        }
        else
        {
            Debug.Log("Click outside boundaries, food not spawned.");
        }
    }

    bool IsWithinBoundaries(Vector3 position)
    {
        // Get boundary dimensions from the DynamicBoundaries script
        float halfWidth = boundaries.boundaryWidth / 2f;
        float halfHeight = boundaries.boundaryHeight / 2f;

        // Check if position is within boundaries
        return position.x >= -halfWidth && position.x <= halfWidth &&
               position.y >= -halfHeight && position.y <= halfHeight;
    }
}
