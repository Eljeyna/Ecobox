using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public ItemDatabase itemDatabase;

    public float weight;
    public List<Item> itemList = new List<Item>();

    public event EventHandler OnItemListChanged;

    public void AddItem(Item item)
    {
        bool itemInInventory = false;
        foreach (Item inventoryItem in itemList)
        {
            if (inventoryItem.id == item.id)
            {
                if (inventoryItem.itemAmount < 999)
                {
                    inventoryItem.itemAmount++;
                }

                itemInInventory = true;
                break;
            }
        }

        if (!itemInInventory)
        {
            itemList.Add(Instantiate(item));
        }

        CallUpdateInventory();
    }

    public void AddItem(Item item, int amount)
    {
        bool itemInInventory = false;
        foreach (Item inventoryItem in itemList)
        {
            if (inventoryItem.id == item.id)
            {
                if (inventoryItem.itemAmount < 999)
                {
                    inventoryItem.itemAmount += amount - 1;
                }

                itemInInventory = true;
                break;
            }
        }

        if (!itemInInventory)
        {
            itemList.Add(Instantiate(item));
            itemList[itemList.Count - 1].itemAmount = amount;
        }

        CallUpdateInventory();
    }

    public void RemoveItem(Item item)
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            if (itemList[i].id == item.id)
            {
                itemList[i].itemInfo.UnloadSprite();
                Destroy(itemList[i]);
                itemList.Remove(itemList[i]);
                break;
            }
        }

        CallUpdateInventory();
    }

    public async Task PreloadInventory()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            await itemList[i].itemInfo.LoadSprite();
        }
    }

    public void UnloadInventory()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            itemList[i].itemInfo.UnloadSprite();
        }
    }

    public void ClearInventory()
    {
        while (itemList.Count > 0)
        {
            itemList[itemList.Count - 1].itemInfo.UnloadSprite();
            Destroy(itemList[itemList.Count - 1]);
            itemList.Remove(itemList[itemList.Count - 1]);
        }
    }

    public void CallUpdateInventory()
    {
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }
}
