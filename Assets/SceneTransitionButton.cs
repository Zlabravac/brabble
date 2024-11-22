using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionButton : MonoBehaviour
{
    [Header("Scene Selection")]
    public string targetScene; // Dropdown for selecting the target scene

    public void TransitionToScene()
    {
        if (!string.IsNullOrEmpty(targetScene))
        {
            SceneManager.LoadScene(targetScene);
        }
        else
        {
            Debug.LogError("Target scene not set for this button!");
        }
    }
}
