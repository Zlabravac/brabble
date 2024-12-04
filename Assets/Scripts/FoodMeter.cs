using UnityEngine;
using UnityEngine.UI;
using System;

public class FoodMeter : MonoBehaviour
{
    [Header("Food Meter Settings")]
    public int maxPortions = 5; // Maximum portions of food
    public float refillRate = 5f; // Time (in seconds) to refill one portion

    [Header("References")]
    public Slider foodMeterSlider;

    private float currentFillAmount;

    void Start()
    {
        LoadFoodMeter();
        UpdateFoodMeterUI();
    }

    void Update()
    {
        // Gradually refill the food meter
        if (currentFillAmount < maxPortions)
        {
            float refillSpeed = Time.deltaTime / refillRate;
            currentFillAmount = Mathf.Clamp(currentFillAmount + refillSpeed, 0, maxPortions);
            UpdateFoodMeterUI();
            SaveFoodMeter(); // Save the updated value
        }
    }

    public bool UseFoodPortion()
    {
        if (currentFillAmount >= 1)
        {
            currentFillAmount -= 1;
            UpdateFoodMeterUI();
            SaveFoodMeter(); // Save after usage
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
        data.foodMeterLastUpdate = DateTime.Now.ToString();
        SaveManager.SaveGame(data);
    }

    private void LoadFoodMeter()
    {
        SaveData data = SaveManager.LoadGame();
        currentFillAmount = data.hungerBarValue > 0 ? data.hungerBarValue : maxPortions;

        // Handle elapsed time for food meter refill
        if (DateTime.TryParse(data.foodMeterLastUpdate, out DateTime lastUpdate))
        {
            double secondsElapsed = (DateTime.Now - lastUpdate).TotalSeconds;
            currentFillAmount = Mathf.Clamp(currentFillAmount + (float)(secondsElapsed / refillRate), 0, maxPortions);
        }
    }

    void OnApplicationQuit()
    {
        SaveFoodMeter();
    }

    void OnDestroy()
    {
        SaveFoodMeter();
    }
}
