using UnityEngine;
using UnityEngine.UI;

public class CoinRateDisplay : MonoBehaviour
{
    public Text coinRateText; // Text component for displaying coin rate
    private int coinsGainedThisSecond = 0; // Coins gained in the current second
    private float timer = 0f; // Timer to reset coin rate display

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1f)
        {
            coinRateText.text = coinsGainedThisSecond.ToString();
            coinsGainedThisSecond = 0;
            timer = 0f;
        }
    }

    public void AddCoins(int amount)
    {
        coinsGainedThisSecond += amount;
    }
}
