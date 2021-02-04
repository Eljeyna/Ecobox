using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName = "ScriptableObjects/Items/Consumable (Health Restore Percent)")]
public class ItemHealthRestorePercent : Item
{
    public float heal;
    public AgilityBuffScriptable testBuff;
    public override void Use()
    {
        if (Player.Instance.thisEntity.healthPercent < 1f)
        {
            Player.Instance.thisEntity.TakeHealthPercent(heal, null);
            Player.Instance.buffSystem.AddBuff(testBuff.InitializeBuff(Player.Instance.gameObject));
            itemAmount--;
        }
    }
}
