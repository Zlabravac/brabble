using System;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    private static string saveFilePath;

    static SaveManager()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "gameSave.json");
    }

    public static void SaveGame(SaveData data)
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(saveFilePath, json);
            Debug.Log($"[SaveManager] Game saved successfully at {saveFilePath}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"[SaveManager] Failed to save game: {ex.Message}");
        }
    }

    public static SaveData LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            try
            {
                string json = File.ReadAllText(saveFilePath);
                Debug.Log("[SaveManager] Game loaded successfully.");
                return JsonUtility.FromJson<SaveData>(json);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[SaveManager] Failed to load game: {ex.Message}");
                return new SaveData(); // Return default data if loading fails
            }
        }
        Debug.Log("[SaveManager] No save file found, creating new data.");
        return new SaveData();
    }
}

[Serializable]
public class SaveData
{
    // General data
    public int money;

    // Hunger system
    public float fishHunger;           // Current hunger value
    public string hungerLastUpdate;    // Last time hunger was updated

    // Food meter system
    public float hungerBarValue;       // Current food meter value
    public string foodMeterLastUpdate; // Last time food meter was updated
}
