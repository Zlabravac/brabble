using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [Header("Food Settings")]
    public GameObject foodPrefab; // The food prefab
    public DinamicBounderies swimArea; // Reference to the fish swim area

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detect left mouse click
        {
            // Get the world position of the mouse click
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Check if the click is within the fish swim area
            if (IsWithinSwimArea(clickPosition))
            {
                SpawnFood(clickPosition);
            }
            else
            {
                Debug.Log("Click outside swim area, food not spawned.");
            }
        }
    }

    private bool IsWithinSwimArea(Vector2 position)
    {
        // Get the swim area boundaries
        float left = -swimArea.boundaryWidth / 2;
        float right = swimArea.boundaryWidth / 2;
        float bottom = -swimArea.boundaryHeight / 2;
        float top = swimArea.boundaryHeight / 2;

        // Check if the position is within the boundaries
        return position.x >= left && position.x <= right && position.y >= bottom && position.y <= top;
    }

    private void SpawnFood(Vector2 position)
    {
        // Instantiate the food prefab at the clicked position
        Instantiate(foodPrefab, position, Quaternion.identity);
        Debug.Log("Food spawned at: " + position);
    }
}
