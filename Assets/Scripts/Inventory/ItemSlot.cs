using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ItemSlot : MonoBehaviour, IPointerClickHandler
{
    //=====ITEM DATA=====//
    public string itemName;
    public int quantity;
    public Texture2D itemTexture;
    public bool isFull;
    public string itemDescription;
    public Sprite emptySprite;

    //=====ITEM SLOT=====//
    [SerializeField]
    private TMP_Text quantityText;

    [SerializeField]
    private Image itemImage;

    //=====ITEM DESCRIPTION SLOT=====//
    public Image itemDescriptionImage;
    public TMP_Text itemDescriptionNameText;
    public TMP_Text itemDescriptionText;

    public GameObject selectedShader;
    public bool thisItemSelected;

    private InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    public void AddItem(string itemName, int quantity, Texture2D itemTexture, string itemDescription)
    {
        this.itemName = itemName;
        this.quantity = quantity;
        this.itemTexture = itemTexture;
        this.itemDescription = itemDescription;
        isFull = true;

        quantityText.text = quantity.ToString();
        quantityText.enabled = true;

        // Convert Texture2D to Sprite
        Rect rect = new Rect(0, 0, itemTexture.width, itemTexture.height);
        Vector2 pivot = new Vector2(0.5f, 0.5f); // Center pivot
        Sprite itemSprite = Sprite.Create(itemTexture, rect, pivot);
        itemImage.sprite = itemSprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick();
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            OnRightClick();
        }
    }

    public void OnLeftClick()
    {
        if (thisItemSelected)
        {
            bool usable = inventoryManager.UseItem(itemName);
            if (usable)
            {
                this.quantity -= 1;
                quantityText.text = this.quantity.ToString();
                if (this.quantity <= 0)
                    EmptySlot();
            }
        }

        else
        {
            inventoryManager.DeselectAllSlots();
            selectedShader.SetActive(true);
            thisItemSelected = true;
            itemDescriptionNameText.text = itemName;
            itemDescriptionText.text = itemDescription;

            // Convert Texture2D to Sprite
            if (itemTexture == null)
            {
                itemDescriptionImage.sprite = emptySprite;
            }
            else
            {
                Rect rect = new Rect(0, 0, itemTexture.width, itemTexture.height);
                Vector2 pivot = new Vector2(0.5f, 0.5f); // Center pivot
                Sprite itemSprite = Sprite.Create(itemTexture, rect, pivot);
                itemDescriptionImage.sprite = itemSprite;
                if (itemDescriptionImage.sprite == null)
                    itemDescriptionImage.sprite = emptySprite;
            }
        }
    }

    public void OnRightClick()
    {
        if (quantity > 0 && thisItemSelected)
        {
            GameObject itemToDrop = inventoryManager.FindPrefab(itemName);
            itemToDrop.GetComponent<Item>().quantity = 1;

            itemToDrop.transform.position = GameObject.FindWithTag("Player").transform.position;
            Instantiate(itemToDrop);

            //Subtract the item
            this.quantity -= 1;
            quantityText.text = this.quantity.ToString();
            isFull = false;
            if (this.quantity <= 0)
                EmptySlot();
        }
    }

    private void EmptySlot()
    {
        // Reset item data
        itemName = "";
        quantity = 0;
        itemTexture = null;
        itemDescription = "";
        isFull = false;

        // Reset UI elements
        //quantityText.enabled = false;
        itemImage.sprite = emptySprite;

        itemDescriptionNameText.text = "";
        itemDescriptionText.text = "";
        itemDescriptionImage.sprite = emptySprite;

        // Deselect the slot
        selectedShader.SetActive(false);
        thisItemSelected = false;
    }
}
