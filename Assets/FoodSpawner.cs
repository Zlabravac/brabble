using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [Header("Food Settings")]
    public GameObject foodPrefab; // The food prefab
    public DinamicBounderies swimArea; // Reference to the fish swim area
    public FoodMeter foodMeter; // Reference to the FoodMeter script

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Detect left mouse click
        {
            if (foodMeter != null && foodMeter.UseFoodPortion()) // Check and use food portion
            {
                Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

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
    }

    private bool IsWithinSwimArea(Vector2 position)
    {
        float left = -swimArea.boundaryWidth / 2;
        float right = swimArea.boundaryWidth / 2;
        float bottom = -swimArea.boundaryHeight / 2;
        float top = swimArea.boundaryHeight / 2;

        return position.x >= left && position.x <= right && position.y >= bottom && position.y <= top;
    }

    private void SpawnFood(Vector2 position)
    {
        Instantiate(foodPrefab, position, Quaternion.identity);
        Debug.Log("Food spawned at: " + position);
    }
}
