using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotPrefab;

    public Color[] colorItems;

    public List<Item> testItems;

    private void Awake()
    {
        itemSlotContainer = gameObject.transform;
        itemSlotPrefab = itemSlotContainer.transform.GetChild(0);
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;

        inventory.OnItemListChanged += Inventory_OnItemListChanged;

        for (int i = 0; i < testItems.Count; i++)
        {
            inventory.AddItem(Instantiate(testItems[i]));
        }

        UpdateInventoryItems();
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        UpdateInventoryItems();
    }

    public void UpdateInventoryItems()
    {
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotPrefab) continue;
            Destroy(child.gameObject);
        }

        int i = 0;
        foreach (ItemInstance item in inventory.itemList)
        {
            i = (int)item.itemCopy.itemQuality;

            RectTransform itemSlotRect = Instantiate(itemSlotPrefab, itemSlotContainer).GetComponent<RectTransform>();
            itemSlotRect.gameObject.SetActive(true);
            itemSlotRect.transform.GetChild(0).GetComponent<ItemInfoSelected>().item = item;
            itemSlotRect.transform.GetChild(0).GetComponent<Image>().color = colorItems[i];
            itemSlotRect.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = item.itemCopy.itemInfo.itemIcon;
            itemSlotRect.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = (item.itemCopy.itemAmount).ToString();
        }
    }
}
