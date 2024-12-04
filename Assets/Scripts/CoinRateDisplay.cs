using UnityEngine;
using UnityEngine.UI;

public class CoinRateDisplay : MonoBehaviour
{
    public Text coinRateText; // The Text component for displaying coin rate
    private int lastMoney = 0; // Money at the start of the last second
    private int moneyEarnedThisSecond = 0; // Money earned in the current second
    private float timer = 0f; // Timer for tracking each second
    public MoneyManager moneyManager; // Reference to the MoneyManager to get current money

    private int initialMoney; // Money from the last save

    void Start()
    {
        // Load saved money and initialize the coin rate text
        SaveData data = SaveManager.LoadGame();
        initialMoney = data.money; // Load the saved money amount
        lastMoney = initialMoney; // Set the starting money
        if (coinRateText != null)
        {
            coinRateText.text = "+0/s";
        }
    }

    void Update()
    {
        // Update the timer
        timer += Time.deltaTime;

        // Check if a second has passed
        if (timer >= 1f)
        {
            // Reset the timer
            timer = 0f;

            // Calculate the money earned in the last second
            if (moneyManager != null)
            {
                int currentMoney = moneyManager.money;

                // Ignore the initial saved money when calculating earned money
                moneyEarnedThisSecond = currentMoney - Mathf.Max(initialMoney, lastMoney);

                // Update the coin rate display
                UpdateCoinRateText(moneyEarnedThisSecond);

                // Store the current money for the next interval
                lastMoney = currentMoney;
            }
        }
    }

    // Public method to trigger immediate coin rate update
    public void TriggerImmediateUpdate()
    {
        if (moneyManager != null)
        {
            int currentMoney = moneyManager.money;

            // Ignore the initial saved money when calculating earned money
            moneyEarnedThisSecond = currentMoney - Mathf.Max(initialMoney, lastMoney);

            // Update the coin rate display
            UpdateCoinRateText(moneyEarnedThisSecond);
        }
    }

    private void UpdateCoinRateText(int coinRate)
    {
        if (coinRateText != null)
        {
            if (coinRate > 0)
            {
                coinRateText.text = "+" + coinRate + "/s";
            }
            else
            {
                coinRateText.text = "+0/s";
            }
        }
    }
}
