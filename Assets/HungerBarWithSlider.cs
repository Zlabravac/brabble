using UnityEngine;
using UnityEngine.UI;

public class HungerBarWithSlider : MonoBehaviour
{
    [Header("Hunger Settings")]
    public float maxHunger = 100f;
    public float hungerDecreaseRate = 1f;

    [Header("UI Components")]
    public Slider hungerSlider;
    public Image hungerFillImage;

    private float currentHunger;

    void Start()
    {
        LoadHunger(); // Load saved hunger value
        hungerSlider.maxValue = maxHunger;
        hungerSlider.value = currentHunger;
    }

    void Update()
    {
        currentHunger -= hungerDecreaseRate * Time.deltaTime;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
        UpdateHungerBar();
    }

    public void ModifyHunger(float amount)
    {
        currentHunger += amount;
        currentHunger = Mathf.Clamp(currentHunger, 0, maxHunger);
        UpdateHungerBar();
        SaveHunger(); // Save the updated hunger value
    }

    public bool IsHungerFull // Restore this property for compatibility
    {
        get
        {
            return currentHunger >= maxHunger;
        }
    }

    private void UpdateHungerBar()
    {
        hungerSlider.value = currentHunger;
        float t = currentHunger / maxHunger;
        hungerFillImage.color = Color.Lerp(Color.red, Color.green, t);
    }

    private void SaveHunger()
    {
        SaveData data = SaveManager.LoadGame();
        data.fishHunger = currentHunger;
        SaveManager.SaveGame(data);
    }

    private void LoadHunger()
    {
        SaveData data = SaveManager.LoadGame();
        currentHunger = data.fishHunger > 0 ? data.fishHunger : maxHunger / 2; // Default to half hunger
    }
}
