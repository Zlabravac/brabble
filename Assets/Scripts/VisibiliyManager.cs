using UnityEngine;
using UnityEngine.SceneManagement;

public class VisibilityManager : MonoBehaviour
{
    public string[] visibleInScenes; // List of scenes where this object should be visible
    private static VisibilityManager instance; // Singleton instance to prevent duplicates

    void Awake()
    {
        // Ensure this object persists across scenes and prevent duplicates
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log($"VisibilityManager: {gameObject.name} is set to DontDestroyOnLoad.");
        }
        else
        {
            Debug.Log($"VisibilityManager: Duplicate instance of {gameObject.name} found and destroyed.");
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        Debug.Log($"VisibilityManager: Initializing visibility for {gameObject.name}.");
        CheckVisibility(); // Check visibility at the start
        SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to scene load events
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"VisibilityManager: Scene changed to {scene.name}. Rechecking visibility for {gameObject.name}.");
        CheckVisibility(); // Recheck visibility when the scene changes
    }

    void CheckVisibility()
    {
        // Get the current scene name
        string currentScene = SceneManager.GetActiveScene().name;
        Debug.Log($"VisibilityManager: Current Scene = {currentScene}");

        // Log all visible scenes for debugging
        Debug.Log($"VisibilityManager: Visible Scenes = {string.Join(", ", visibleInScenes)}");

        // Check if the current scene is in the visible scenes list
        bool shouldBeVisible = System.Array.Exists(visibleInScenes, scene => scene == currentScene);
        Debug.Log($"VisibilityManager: Should {gameObject.name} be visible? {shouldBeVisible}");

        // Update the object's active state
        if (gameObject.activeSelf != shouldBeVisible)
        {
            Debug.Log($"VisibilityManager: Setting active state of {gameObject.name} to {shouldBeVisible}");
            gameObject.SetActive(shouldBeVisible);
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from scene load events to avoid errors
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Debug.Log($"VisibilityManager: {gameObject.name} destroyed.");
    }
}
