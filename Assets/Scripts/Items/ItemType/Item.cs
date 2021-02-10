using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "ScriptableObjects/Items/New Item")]
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
    public AssetReference idReference;

    public bool itemEnd = true;
    public float itemWeight;
    public int itemCost;
    public int itemAmount = 1;
    public float chanceDrop;
    
    public ItemType itemType;
    public ItemQuality itemQuality;
    public ItemInfo itemInfo;

    public virtual void Use() {}

    private void OnDestroy()
    {
        if (idReference.IsValid())
        {
            Addressables.Release(idReference);
        }
    }
}
