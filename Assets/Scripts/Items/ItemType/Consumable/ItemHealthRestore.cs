using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName = "ScriptableObjects/Items/Consumable (Health Restore)")]
public class ItemHealthRestore : Item
{
    public float heal;
    public override void Use()
    {
        if (Player.Instance.thisPlayer.healthPercent < 1f)
        {
            Player.Instance.thisPlayer.TakeHealth(heal, null);
            itemAmount--;
        }
    }
}
