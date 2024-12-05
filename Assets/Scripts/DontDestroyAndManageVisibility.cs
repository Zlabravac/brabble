using UnityEngine;

public class DontDestroyAndManageVisibility : MonoBehaviour
{
    private static DontDestroyAndManageVisibility instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist the object
        }
        else
        {
            Destroy(gameObject); // Prevent duplicates
        }
    }
}
