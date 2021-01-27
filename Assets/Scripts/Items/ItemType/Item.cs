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
    public float itemWeight;
    public int itemCost;
    public int itemAmount = 1;
    
    public ItemType itemType;
    public ItemQuality itemQuality;
    public ItemInfo itemInfo;

    public virtual void Use() {}
}
