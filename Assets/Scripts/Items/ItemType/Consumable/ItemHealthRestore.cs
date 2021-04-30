using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Items/ItemType/Consumable/Consumable (Health Restore)")]
public class ItemHealthRestore : Item
{
    public float heal;
    public override void Use()
    {
        if (Player.Instance.thisEntity.healthPercent < 1f)
        {
            Player.Instance.thisEntity.TakeHealth(heal, null);
            itemAmount--;
        }
    }
}
