using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public List<Item> testItems;

    public TMP_Text weightText;

    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotPrefab;

    private bool DEBUG = true;

    private void Awake()
    {
        itemSlotContainer = gameObject.transform;
        itemSlotPrefab = itemSlotContainer.transform.GetChild(0);
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;

        inventory.OnItemListChanged += Inventory_OnItemListChanged;

        if (DEBUG)
        {
            for (int i = 0; i < testItems.Count; i++)
            {
                inventory.AddItem(testItems[i]);
            }
        }
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        UpdateInventoryItems();
    }

    public void UpdateInventoryItems()
    {
        inventory.weight = 0f;
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotPrefab) continue;
            Destroy(child.gameObject);
        }

        int i = 0;
        foreach (Item item in inventory.itemList)
        {
            inventory.weight += item.itemWeight * item.itemAmount;

            if (item.itemType == StaticGameVariables.currentItemCategory)
            {
                i = (int)item.itemQuality;

                RectTransform itemSlotRect = Instantiate(itemSlotPrefab, itemSlotContainer).GetComponent<RectTransform>();
                itemSlotRect.gameObject.SetActive(true);

                ItemInfoSelected itemInfo = itemSlotRect.transform.GetChild(0).GetComponent<ItemInfoSelected>();

                itemInfo.item = item;
                if (StaticGameVariables.itemSelected == item)
                {
                    itemInfo.GetItemInfo();
                }

                itemSlotRect.transform.GetChild(0).GetComponent<Image>().color = StaticGameVariables.colorItems[i];
                itemSlotRect.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = item.itemInfo.itemIcon;
                itemSlotRect.transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TMP_Text>().text = (item.itemAmount).ToString();
            }
        }

        weightText.text = ($"{Mathf.Round(inventory.weight)} / {Player.Instance.stats.weight}");
    }
}
