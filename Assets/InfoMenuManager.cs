using UnityEngine;
using UnityEngine.UI;

public class InfoMenuManager : MonoBehaviour
{
    public GameObject fishSectionPrefab;  // Assign the FishSection prefab
    public Transform contentParent;       // Assign the Content object

    void Start()
    {
        PopulateMenu();
    }

    void PopulateMenu()
    {
        // Clear any existing menu items
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        // Find all fish in the scene
        FishStats[] allFish = FindObjectsOfType<FishStats>();

        // Create a menu section for each fish
        foreach (FishStats fish in allFish)
        {
            // Instantiate a new section
            GameObject newSection = Instantiate(fishSectionPrefab, contentParent);

            // Update the fields with the fish's stats
            newSection.transform.Find("NameText").GetComponent<Text>().text = "Name: " + fish.fishName;
            newSection.transform.Find("RarityText").GetComponent<Text>().text = "Rarity: " + fish.rarity;
            newSection.transform.Find("SizeText").GetComponent<Text>().text = "Size: " + fish.size;
            newSection.transform.Find("MoneyText").GetComponent<Text>().text = "Money/s: " + fish.moneyPerSecond;

            // Set the fish image
            newSection.transform.Find("FishImage").GetComponent<Image>().sprite = fish.fishSprite;
        }
    }
}
