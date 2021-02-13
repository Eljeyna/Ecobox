using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName = "ScriptableObjects/Items/ItemType/Comsumable/Consumable (Health Restore Percent)")]
public class ItemHealthRestorePercent : Item
{
    public float heal;
    
    public override void Use()
    {
        if (Player.Instance.thisEntity.healthPercent < 1f)
        {
            Player.Instance.thisEntity.TakeHealthPercent(heal, null);
            itemAmount--;
        }
    }
}
