using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomInfoMenuManager : MonoBehaviour
{
    public GameObject objectPrefab; // Prefab for each item in the menu
    public Transform contentParent; // Parent for all instantiated items
    public List<ObjectStats> objectList = new List<ObjectStats>(); // List of items to display

    void Start()
    {
        if (objectPrefab == null)
        {
            Debug.LogError("objectPrefab is null! Check prefab assignment.");
            return;
        }

        if (contentParent == null)
        {
            Debug.LogError("contentParent is null! Check hierarchy assignment.");
            return;
        }

        PopulateInfoMenu();
    }

    void PopulateInfoMenu()
    {
        foreach (Transform child in contentParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var obj in objectList)
        {
            if (obj == null)
            {
                Debug.LogError("One of the objects in the objectList is null! Check the list assignment.");
                continue;
            }

            GameObject newItem = Instantiate(objectPrefab, contentParent);
            if (newItem == null)
            {
                Debug.LogError("newItem failed to instantiate! Check objectPrefab.");
                continue;
            }

            // Assign Photo
            Image photoImage = newItem.transform.Find("Photo").GetComponent<Image>();
            if (photoImage != null)
            {
                photoImage.sprite = obj.photo;
            }
            else
            {
                Debug.LogError("Photo Image component is missing from the prefab!");
            }

            // Assign Storage Capacity
            Text storageText = newItem.transform.Find("StorageCapacity").GetComponent<Text>();
            if (storageText != null)
            {
                storageText.text = $"Storage: {obj.storageCapacity}";
            }
            else
            {
                Debug.LogError("StorageCapacity Text component is missing from the prefab!");
            }

            // Assign Size
            Text sizeText = newItem.transform.Find("Size").GetComponent<Text>();
            if (sizeText != null)
            {
                sizeText.text = $"Size: {obj.size} units";
            }
            else
            {
                Debug.LogError("Size Text component is missing from the prefab!");
            }

            Debug.Log($"Added item: Storage = {obj.storageCapacity}, Size = {obj.size}");
        }
    }
}
