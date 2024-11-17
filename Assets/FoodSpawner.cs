using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject foodPrefab; // The food prefab
    public Camera mainCamera; // Reference to the camera

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

        // Spawn the food prefab at the clicked position
        Instantiate(foodPrefab, worldPosition, Quaternion.identity);
    }
}
