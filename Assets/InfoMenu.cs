using UnityEngine;
using UnityEngine.EventSystems;

public class InfoMenu : MonoBehaviour
{
    [Header("Menu Settings")]
    public GameObject menu; // The menu GameObject
    public CanvasGroup gameUI; // The CanvasGroup for the game UI
    public GameObject feedingManager; // The GameObject managing feeding (disable when menu is open)

    private bool isMenuOpen = false; // Tracks the menu state

    void Start()
    {
        // Ensure the menu is initially closed
        if (menu != null)
        {
            menu.SetActive(false);
        }

        Debug.Log("Info Menu initialized: Closed");
    }

    public void ToggleMenu()
    {
        if (isMenuOpen)
        {
            Debug.Log("ToggleMenu: Closing Info Menu");
            CloseMenu();
        }
        else
        {
            Debug.Log("ToggleMenu: Opening Info Menu");
            OpenMenu();
        }
    }

    public void OpenMenu()
    {
        if (menu != null)
        {
            // Show the menu
            menu.SetActive(true);

            // Disable game UI and gameplay interactions
            DisableGameUI();
            DisableGameplay();

            isMenuOpen = true;
            Debug.Log("Info Menu opened");
        }
    }

    public void CloseMenu()
    {
        if (menu != null)
        {
            // Hide the menu
            menu.SetActive(false);

            // Re-enable game UI and gameplay interactions
            EnableGameUI();
            EnableGameplay();

            isMenuOpen = false;
            Debug.Log("Info Menu closed");
        }
    }

    private void DisableGameUI()
    {
        if (gameUI != null)
        {
            gameUI.blocksRaycasts = false; // Prevent clicks on other UI elements
            gameUI.interactable = false;  // Disable interactions
            Debug.Log("Game UI disabled for Info Menu");
        }
    }

    private void EnableGameUI()
    {
        if (gameUI != null)
        {
            gameUI.blocksRaycasts = true; // Allow clicks on other UI elements
            gameUI.interactable = true;  // Enable interactions
            Debug.Log("Game UI enabled for Info Menu");
        }
    }

    private void DisableGameplay()
    {
        if (feedingManager != null)
        {
            feedingManager.SetActive(false); // Disable feeding functionality
            Debug.Log("Gameplay disabled for Info Menu");
        }
    }

    private void EnableGameplay()
    {
        if (feedingManager != null)
        {
            feedingManager.SetActive(true); // Enable feeding functionality
            Debug.Log("Gameplay enabled for Info Menu");
        }
    }

    void Update()
    {
        if (isMenuOpen && Input.GetMouseButtonDown(0))
        {
            // Detect where the click occurred
            if (IsClickOutsideMenu())
            {
                Debug.Log("Click detected outside the Info Menu. Closing menu.");
                CloseMenu();
            }
            else
            {
                Debug.Log("Click detected on the Info Menu or its elements. Menu remains open.");
            }
        }
    }

    private bool IsClickOutsideMenu()
    {
        // Detect if the click is not on the menu or its child elements
        if (menu != null)
        {
            RectTransform menuRect = menu.GetComponent<RectTransform>();
            if (menuRect != null)
            {
                Vector2 mousePosition = Input.mousePosition;
                // Check if the click is outside the menu bounds
                if (RectTransformUtility.RectangleContainsScreenPoint(menuRect, mousePosition, Camera.main))
                {
                    return false; // Click is on the menu
                }
            }
        }

        // Check if the click is on any other UI elements
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return false; // Click is on some other UI element
        }

        return true; // Click is outside the menu and other UI elements
    }
}
