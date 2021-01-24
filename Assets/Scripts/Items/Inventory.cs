using System;
using System.Collections.Generic;

public class Inventory
{
    public float weight;
    public List<ItemInstance> itemList = new List<ItemInstance>();

    public event EventHandler OnItemListChanged;

    public void AddItem(Item item)
    {
        bool itemInInventory = false;
        foreach (ItemInstance inventoryItem in itemList)
        {
            if (inventoryItem.itemCopy.id == item.id)
            {
                if (inventoryItem.itemCopy.itemAmount < 999)
                {
                    inventoryItem.itemCopy.itemAmount++;
                }
                itemInInventory = true;
                break;
            }
        }

        if (!itemInInventory)
        {
            itemList.Add(new ItemInstance(item));
        }

        CallUpdateInventory();
    }

    public void AddItem(ItemInstance item)
    {
        bool itemInInventory = false;
        foreach (ItemInstance inventoryItem in itemList)
        {
            if (inventoryItem.itemCopy.id == item.itemCopy.id)
            {
                if (inventoryItem.itemCopy.itemAmount < 999)
                {
                    inventoryItem.itemCopy.itemAmount++;
                }
                itemInInventory = true;
                break;
            }
        }

        if (!itemInInventory)
        {
            itemList.Add(new ItemInstance(item.itemCopy));
        }

        CallUpdateInventory();
    }

    public void RemoveItem(Item item)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].itemCopy.id == item.id)
            {
                itemList.Remove(itemList[i]);
                break;
            }
        }

        CallUpdateInventory();
    }

    public void RemoveItem(ItemInstance item)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].itemCopy.id == item.itemCopy.id)
            {
                itemList.Remove(itemList[i]);
                break;
            }
        }

        CallUpdateInventory();
    }

    public void CallUpdateInventory()
    {
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }
}
