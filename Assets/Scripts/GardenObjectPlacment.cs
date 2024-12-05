using UnityEngine;

public class GardenObjectPlacement : MonoBehaviour
{
    public GameObject objectToPlace; // The prefab to place
    public int objectCost = 50; // Cost of the object

    private MoneyManager moneyManager;

    void Start()
    {
        // Find the MoneyManager in the scene
        moneyManager = FindObjectOfType<MoneyManager>();
        if (moneyManager == null)
        {
            Debug.LogError("MoneyManager not found in the scene.");
        }
    }

    public void TryPlaceObject(Vector3 position)
    {
        if (moneyManager != null && moneyManager.SpendMoney(objectCost))
        {
            // Place the object if purchase is successful
            Instantiate(objectToPlace, position, Quaternion.identity);
            Debug.Log("Object placed successfully.");
        }
        else
        {
            Debug.Log("Not enough money to place the object.");
        }
    }
}
