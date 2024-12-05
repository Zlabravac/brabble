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

    public bool SpendMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            SaveMoney(); // Save the updated money value
            UpdateMoneyText();
            return true; // Purchase successful
        }
        else
        {
            Debug.Log("Not enough money to complete the purchase.");
            return false; // Purchase failed
        }
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
