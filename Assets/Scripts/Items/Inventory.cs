using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public Dictionary<string, Item> itemList = new Dictionary<string, Item>();
    private List<string> itemListToDestroy = new List<string>();

    public event EventHandler OnItemListChanged;

    public void AddItem(Item item)
    {
        if (itemList.TryGetValue(item.itemName, out Item value))
        {
            value.itemAmount += Mathf.Clamp(item.itemAmount, 1, 999);
        }
        else
        {
            itemList.Add(item.itemName, Instantiate(item));
        }
    }

    public void AddItem(Item item, int amount)
    {
        if (itemList.TryGetValue(item.itemName, out Item value))
        {
            value.itemAmount += Mathf.Clamp(amount, 1, 999);
        }
        else
        {
            itemList.Add(item.itemName, Instantiate(item));
        }
    }

    public void RemoveItem(Item item)
    {
        itemList[item.itemName].UnloadSprite();
        Destroy(itemList[item.itemName]);
        itemList.Remove(item.itemName);
    }

    public async Task PreloadInventory()
    {
        foreach (string key in itemList.Keys)
        {
            await itemList[key].LoadSprite();
        }
    }

    public void UnloadInventory()
    {
        foreach (string key in itemList.Keys)
        {
            itemList[key].UnloadSprite();
        }

        Player.Instance.inventoryUI.RemoveInventorySlots();
    }

    public void ClearInventory()
    {
        itemListToDestroy.Clear();
        foreach (string key in itemList.Keys)
        {
            itemList[key].UnloadSprite();
            Destroy(itemList[key]);
            itemListToDestroy.Add(key);
        }
        
        for (int i = itemListToDestroy.Count - 1; i >= 0; i--)
        {
            itemList.Remove(itemListToDestroy[i]);
        }
    }

    public void CallUpdateInventory()
    {
        OnItemListChanged?.Invoke(this, EventArgs.Empty);
    }
}
