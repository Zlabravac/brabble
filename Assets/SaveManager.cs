using System.IO;
using UnityEngine;

public static class SaveManager
{
    private static string saveFilePath;

    static SaveManager()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "gameSave.json");
        Debug.Log($"[SaveManager] Save file path: {saveFilePath}");
    }

    public static void SaveGame(SaveData data)
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(saveFilePath, json);
            Debug.Log("[SaveManager] Game saved successfully.");
        }
        catch (System.Exception ex)
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
                return JsonUtility.FromJson<SaveData>(json);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[SaveManager] Failed to load game: {ex.Message}");
            }
        }
        return new SaveData(); // Default values
    }
}

[System.Serializable]
public class SaveData
{
    public int money;
    public float fishHunger;
    public float hungerBarValue;
}
