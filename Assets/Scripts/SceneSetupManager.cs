using UnityEngine;

public class SceneSetupManager : MonoBehaviour
{
    public GameObject[] objectsToActivate; // List of objects to enable for this scene

    void Start()
    {
        foreach (GameObject obj in objectsToActivate)
        {
            obj.SetActive(true); // Activate specific objects
        }
    }
}
