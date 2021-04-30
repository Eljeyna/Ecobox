using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Items/ItemType/Consumable/Consumable (Health Restore Percent)")]
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
