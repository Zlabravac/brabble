using UnityEngine;
using UnityEngine.UI;
using System;

public class HungerBarWithSlider : MonoBehaviour
{
    [Header("Hunger Settings")]
    public float maxHunger = 100f;
    public float hungerDecreaseRate = 1f; // Per second

    [Header("UI Components")]
    public Slider hungerSlider;
    public Image hungerFillImage;

    private float currentHunger;

    void Start()
    {
        LoadHunger();
        hungerSlider.maxValue = maxHunger;
        hungerSlider.value = currentHunger;
    }

    void Update()
    {
        // Reduce hunger over time
        currentHunger = Mathf.Clamp(currentHunger - hungerDecreaseRate * Time.deltaTime, 0, maxHunger);
        UpdateHungerBar();
    }

    private void UpdateHungerBar()
    {
        hungerSlider.value = currentHunger;
        float t = currentHunger / maxHunger;
        hungerFillImage.color = Color.Lerp(Color.red, Color.green, t);
    }

    public void ModifyHunger(float amount)
    {
        currentHunger += amount;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
        UpdateHungerBar();
        SaveHunger();
    }

    public bool IsHungerFull
    {
        get
        {
            return currentHunger >= maxHunger;
        }
    }

    private void SaveHunger()
    {
        SaveData data = SaveManager.LoadGame();
        data.fishHunger = currentHunger;
        data.hungerLastUpdate = DateTime.Now.ToString();
        SaveManager.SaveGame(data);
    }

    private void LoadHunger()
    {
        SaveData data = SaveManager.LoadGame();
        currentHunger = data.fishHunger > 0 ? data.fishHunger : maxHunger / 2; // Default to half hunger

        // Handle elapsed time for hunger depletion
        if (DateTime.TryParse(data.hungerLastUpdate, out DateTime lastUpdate))
        {
            double secondsElapsed = (DateTime.Now - lastUpdate).TotalSeconds;
            currentHunger = Mathf.Clamp(currentHunger - (float)(secondsElapsed * hungerDecreaseRate), 0, maxHunger);
        }
    }

    void OnApplicationQuit()
    {
        SaveHunger();
    }

    void OnDestroy()
    {
        SaveHunger();
    }
}
