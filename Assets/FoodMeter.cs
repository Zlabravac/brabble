using UnityEngine;
using UnityEngine.UI;

public class FoodMeter : MonoBehaviour
{
    [Header("Food Meter Settings")]
    public int maxPortions = 5; // Total portions in the meter
    public float refillRate = 1f; // Time in seconds to refill one portion
    public Slider foodMeterSlider; // UI Slider for the food meter

    private int currentPortions; // Current portions available
    private float refillTimer; // Timer for refilling portions

    void Start()
    {
        // Initialize the meter
        currentPortions = maxPortions;
        UpdateFoodMeterUI();
    }

    void Update()
    {
        // Refill portions over time
        if (currentPortions < maxPortions)
        {
            refillTimer += Time.deltaTime;
            if (refillTimer >= refillRate)
            {
                currentPortions++;
                refillTimer = 0f;
                UpdateFoodMeterUI();
            }
        }
    }

    public bool UseFoodPortion()
    {
        // Check if a portion is available
        if (currentPortions > 0)
        {
            currentPortions--;
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
        // Update the slider value to match the current portions
        if (foodMeterSlider != null)
        {
            foodMeterSlider.maxValue = maxPortions;
            foodMeterSlider.value = currentPortions;
        }
    }
}
