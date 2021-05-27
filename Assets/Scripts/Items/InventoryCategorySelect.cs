using UnityEngine;

public class InventoryCategorySelect : MonoBehaviour
{
    public Item.ItemType itemType;

    public void Use()
    {
        Game.ChangeCategoryItem(itemType);
    }
}
