using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "ScriptableObjects/Items/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    public List<Item> allItems;

    public Item GetItem(int id)
    {
        foreach (Item item in allItems)
        {
            if (item.id == id)
            {
                return item;
            }
        }

        return null;
    }
}