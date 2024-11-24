using UnityEngine;
using UnityEngine.UI;

public class FoodMeter : MonoBehaviour
{
    [Header("Food Meter Settings")]
    public int maxPortions = 5;
    public float refillRate = 5f;

    [Header("References")]
    public Slider foodMeterSlider;

    private float currentFillAmount;

    void Start()
    {
        LoadFoodMeter(); // Load saved food meter value
        UpdateFoodMeterUI();
    }

    void Update()
    {
        if (currentFillAmount < maxPortions)
        {
            float refillSpeed = (maxPortions / refillRate) * Time.deltaTime;
            currentFillAmount = Mathf.Min(maxPortions, currentFillAmount + refillSpeed);
            UpdateFoodMeterUI();
            SaveFoodMeter(); // Save the updated food meter value
        }
    }

    public bool UseFoodPortion()
    {
        if (currentFillAmount >= 1)
        {
            currentFillAmount -= 1;
            UpdateFoodMeterUI();
            SaveFoodMeter(); // Save after using a portion
            return true;
        }
        else
        {
            Debug.Log("No food portions available!");
            return false;
        }
    }

    private void UpdateFoodMeterUI()
    {
        if (foodMeterSlider != null)
        {
            foodMeterSlider.maxValue = maxPortions;
            foodMeterSlider.value = currentFillAmount;
        }
    }

    private void SaveFoodMeter()
    {
        SaveData data = SaveManager.LoadGame();
        data.hungerBarValue = currentFillAmount;
        SaveManager.SaveGame(data);
    }

    private void LoadFoodMeter()
    {
        SaveData data = SaveManager.LoadGame();
        currentFillAmount = data.hungerBarValue > 0 ? data.hungerBarValue : maxPortions;
    }
}
