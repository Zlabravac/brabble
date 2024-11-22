using UnityEngine;
using UnityEngine.UI;

public class FoodMeter : MonoBehaviour
{
    [Header("Food Meter Settings")]
    public int maxPortions = 5; // Total portions in the meter
    public float refillRate = 5f; // Time in seconds to fully refill the bar

    [Header("References")]
    public Slider foodMeterSlider; // UI Slider for the food meter

    private float currentFillAmount; // Current fill amount, gradually increasing
    private float portionSize; // Amount for one portion of the bar

    void Start()
    {
        // Initialize the meter
        currentFillAmount = maxPortions;
        portionSize = maxPortions; // Slider max value corresponds to max portions
        UpdateFoodMeterUI();
    }

    void Update()
    {
        // Gradually refill the meter over time
        if (currentFillAmount < maxPortions)
        {
            float refillSpeed = (maxPortions / refillRate) * Time.deltaTime; // Calculate gradual refill speed
            currentFillAmount = Mathf.Min(maxPortions, currentFillAmount + refillSpeed);
            UpdateFoodMeterUI();
        }
    }

    public bool UseFoodPortion()
    {
        // Check if at least one portion is available
        if (currentFillAmount >= 1)
        {
            // Reduce the meter by one portion
            currentFillAmount -= 1;
            UpdateFoodMeterUI();
            return true; // Portion used successfully
        }
        else
        {
            Debug.Log("No food portions available!");
            return false; // No portions left
        }
    }

    private void UpdateFoodMeterUI()
    {
        // Update the slider value to match the current fill amount
        if (foodMeterSlider != null)
        {
            foodMeterSlider.maxValue = maxPortions;
            foodMeterSlider.value = currentFillAmount;
        }
    }
}
