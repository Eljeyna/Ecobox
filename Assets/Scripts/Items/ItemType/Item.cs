using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Items/Item")]
public class Item : ScriptableObject
{
    public enum ItemType
    {
        WeaponMelee,
        WeaponRanged,
        Cloth,
        Consumable,
        Trash,
        Quest
    }

    public enum ItemQuality
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary,
        Relic
    }

    public int id;

    public bool itemEnd = true;
    public float itemWeight = 0.0f;
    public int itemCost = 0;
    public int itemAmount = 1;
    
    public ItemType itemType;
    public ItemQuality itemQuality;
    public ItemInfo itemInfo;

    public virtual void Use() {}
}

[System.Serializable]
public class ItemInstance
{
    public Item itemCopy;

    public ItemInstance(Item item)
    {
        this.itemCopy = item;
    }
}
