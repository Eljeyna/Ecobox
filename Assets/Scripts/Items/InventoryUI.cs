using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public TMP_Text weightText;

    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotPrefab;

    private void Awake()
    {
        itemSlotContainer = gameObject.transform;
        itemSlotPrefab = itemSlotContainer.transform.GetChild(0);
    }

    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;

        inventory.OnItemListChanged += Inventory_OnItemListChanged;
    }

    private void Inventory_OnItemListChanged(object sender, System.EventArgs e)
    {
        UpdateInventoryItems();
    }

    public void UpdateInventoryItems()
    {
        inventory.weight = 0f;
        RemoveInventorySlots();

        int i = 0;
        foreach (Item item in inventory.itemList)
        {
            inventory.weight += item.itemWeight * item.itemAmount;

            if (item.itemType == StaticGameVariables.currentItemCategory)
            {
                i = (int)item.itemQuality;

                RectTransform itemSlotRect = Instantiate(itemSlotPrefab, itemSlotContainer).GetComponent<RectTransform>();
                itemSlotRect.gameObject.SetActive(true);

                ItemInfoSelected itemInfo = null;
                if (itemSlotRect.transform.GetChild(0).TryGetComponent(out ItemInfoSelected newItemInfo))
                {
                    itemInfo = newItemInfo;
                }

                itemInfo.item = item;
                if (StaticGameVariables.itemSelected == item)
                {
                    itemInfo.GetItemInfo();
                }

                if (itemSlotRect.transform.GetChild(0).TryGetComponent(out Image newImage))
                {
                    newImage.color = StaticGameVariables.colorItems[i];
                }

                if (itemSlotRect.transform.GetChild(0).GetChild(0).TryGetComponent(out Image newSprite))
                {
                    newSprite.sprite = item.itemInfo.itemIcon;
                }

                if (itemSlotRect.transform.GetChild(0).GetChild(1).GetChild(0).TryGetComponent(out TMP_Text newText))
                {
                    newText.text = (item.itemAmount).ToString();
                }
            }
        }

        weightText.text = ($"{Mathf.Round(inventory.weight)} / {Player.Instance.stats.weight}");
    }

    public void RemoveInventorySlots()
    {
        foreach (Transform child in itemSlotContainer)
        {
            if (child == itemSlotPrefab) continue;
            Destroy(child.gameObject);
        }
    }
}
