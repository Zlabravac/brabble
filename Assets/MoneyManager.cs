using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public Text moneyText; // Reference to the UI Text element
    public int money = 0;  // Player's total money
    public CoinRateDisplay coinRateDisplay; // Reference to the CoinRateDisplay script

    public void AddMoney(int amount)
    {
        money += amount; // Add the specified amount of money
        UpdateMoneyText(); // Update the UI text

        // Trigger immediate coin rate update
        if (coinRateDisplay != null)
        {
            coinRateDisplay.TriggerImmediateUpdate();
        }
    }

    private void UpdateMoneyText()
    {
        moneyText.text = "Money: " + money; // Update the text to show current money
    }
}
