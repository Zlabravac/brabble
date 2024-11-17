using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public Text moneyText; // Reference to the UI Text element
    public int money = 0;  // Player's money

    // Method to add money
    public void AddMoney(int amount)
    {
        money += amount; // Add the specified amount of money
        UpdateMoneyText(); // Update the UI text
    }

    // Method to update the money text
    private void UpdateMoneyText()
    {
        moneyText.text = "Money: " + money; // Update the text to show current money
    }

    void Start()
    {
        // Initialize the money text
        UpdateMoneyText();
    }
}
