using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;
    public int quantity;
    public Texture2D itemTexture;
    [TextArea]
    public string itemDescription;
    public float interactionRange = 10f; // The maximum distance the player can pick up items
    public LayerMask interactableLayer; // Layer mask for interactable objects

    private InventoryManager inventoryManager;
    private bool isPickedUp = false; // Flag to prevent multiple pickups

    // Start is called before the first frame update
    void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    void Update()
    {
        // Check for interaction key press
        if (Input.GetKeyDown(KeyCode.E))
        {
            AttemptPickup();
        }
    }

    void AttemptPickup()
    {
        // Raycast from the camera (or player) in the forward direction
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactionRange, interactableLayer))
        {
            // Check if the hit object has the "Item" component
            Item hitItem = hit.collider.GetComponent<Item>();
            if (hitItem != null && !hitItem.isPickedUp)
            {
                // Mark the item as picked up to prevent multiple pickups
                hitItem.isPickedUp = true;

                // Add the item to the inventory
                inventoryManager.AddItem(hitItem.itemName, hitItem.quantity, hitItem.itemTexture, hitItem.itemDescription);

                // Destroy the item game object
                Destroy(hitItem.gameObject);
            }
        }
    }
}
