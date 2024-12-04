using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public Text moneyText;
    public int money = 0;

    void Start()
    {
        LoadMoney(); // Load saved money value
        UpdateMoneyText();
    }

    public void AddMoney(int amount)
    {
        money += amount;
        SaveMoney(); // Save the updated money value
        UpdateMoneyText();
    }

    private void UpdateMoneyText()
    {
        moneyText.text = "Money: " + money;
    }

    private void SaveMoney()
    {
        SaveData data = SaveManager.LoadGame();
        data.money = money;
        SaveManager.SaveGame(data);
    }

    private void LoadMoney()
    {
        SaveData data = SaveManager.LoadGame();
        money = data.money;
    }
}
