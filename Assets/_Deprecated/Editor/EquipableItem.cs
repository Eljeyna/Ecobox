using UnityEngine;

public enum ItemClothType
{
    Head    = 0,
    Torso   = 1,
    Legs    = 2,
    Foots   = 3,
}
/*
[CreateAssetMenu(menuName = "ScriptableObjects/Items/Cloth/New Cloth Item")]
public class EquipableItem : Item
{
    public ItemClothType itemClothType;
    public ScriptableObjectBuff itemBuff;
    
    public override void Use()
    {
        EquipableItem itemEquipped;
        
        switch (itemClothType)
        {
            case ItemClothType.Head:
                itemEquipped = Player.Instance.head;
                break;
            case ItemClothType.Torso:
                itemEquipped = Player.Instance.torso;
                break;
            case ItemClothType.Legs:
                itemEquipped = Player.Instance.legs;
                break;
            case ItemClothType.Foots:
                itemEquipped = Player.Instance.foots;
                break;
            default:
                return;
        }

        if (itemEquipped == this)
        {
            Player.Instance.buffSystem.RemoveBuff(itemEquipped.itemBuff.GetBuff());
            itemEquipped = null;
        }
        else if (itemEquipped)
        {
            Player.Instance.buffSystem.RemoveBuff(itemEquipped.itemBuff.GetBuff());
            Player.Instance.buffSystem.AddBuff(itemBuff.InitializeBuff(Player.Instance.gameObject));
            itemEquipped = this;
        }
        else
        {
            Player.Instance.buffSystem.AddBuff(itemBuff.InitializeBuff(Player.Instance.gameObject));
            itemEquipped = this;
        }
        
        switch (itemClothType)
        {
            case ItemClothType.Head:
                Player.Instance.head = itemEquipped;
                break;
            case ItemClothType.Torso:
                Player.Instance.torso = itemEquipped;
                break;
            case ItemClothType.Legs:
                Player.Instance.legs = itemEquipped;
                break;
            case ItemClothType.Foots:
                Player.Instance.foots = itemEquipped;
                break;
            default:
                return;
        }
    }
}
*/