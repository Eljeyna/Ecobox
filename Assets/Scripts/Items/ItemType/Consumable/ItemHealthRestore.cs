using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName = "ScriptableObjects/Items/Consumable")]
public class ItemHealthRestore : Item
{
    public float heal;
    public override void Use()
    {
        StaticGameVariables.player.GetComponent<BaseEntity>().TakeHealthPercent(heal, null);
        itemAmount--;
    }
}
