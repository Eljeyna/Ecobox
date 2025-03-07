using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public TMP_Text qualitativeMaterialCount;
    public TMP_Text badQualityMaterialCount;

    private Inventory inventory;
    private Transform itemSlotContainer;
    private Transform itemSlotPrefab;

    public void Initialize()
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

    public async void UpdateInventoryItems()
    {
        RemoveInventorySlots();

        foreach (Item item in inventory.itemList.Values)
        {
            if (item.itemType == Game.currentItemCategory)
            {
                RectTransform itemSlotRect = Instantiate(itemSlotPrefab, itemSlotContainer).GetComponent<RectTransform>();
                itemSlotRect.gameObject.SetActive(true);

                if (itemSlotRect.transform.GetChild(0).TryGetComponent(out ItemInfoSelected newItemInfo))
                {
                    ItemInfoSelected itemInfo = newItemInfo;
                    
                    itemInfo.item = item;
                    if (Game.itemSelected == item)
                    {
                        itemInfo.GetItemInfo();
                    }
                }

                if (itemSlotRect.transform.GetChild(0).TryGetComponent(out Image newImage))
                {
                    newImage.color = Game.colorItems[(int)item.itemQuality];
                }

                if (itemSlotRect.transform.GetChild(0).GetChild(0).TryGetComponent(out Image newSprite))
                {
                    newSprite.sprite = await item.LoadSprite();
                }

                if (itemSlotRect.transform.GetChild(0).GetChild(1).GetChild(0).TryGetComponent(out TMP_Text newText))
                {
                    newText.text = item.itemAmount.ToString();
                }
            }
        }
    }

    public void RemoveInventorySlots()
    {
        if (itemSlotContainer.transform.childCount > 0)
        {
            foreach (Transform child in itemSlotContainer)
            {
                if (child == itemSlotPrefab) continue;
                Destroy(child.gameObject);
            }
        }
    }
}
