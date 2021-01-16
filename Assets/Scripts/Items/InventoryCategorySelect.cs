using UnityEngine;

public class InventoryCategorySelect : MonoBehaviour
{
    public Item.ItemType itemType;

    public void Use()
    {
        StaticGameVariables.ChangeCategoryItem(itemType);
    }
}
