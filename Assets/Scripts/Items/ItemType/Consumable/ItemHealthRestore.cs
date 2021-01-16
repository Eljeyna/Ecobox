using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName = "ScriptableObjects/Items/Consumable")]
public class ItemHealthRestore : Item
{
    public override void Use()
    {
        itemAmount--;
    }
}
