using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName = "ScriptableObjects/Items/Consumable (Health Restore Percent)")]
public class ItemHealthRestorePercent : Item
{
    public float heal;
    public AgilityBuffScriptable testBuff;
    public override void Use()
    {
        if (Player.Instance.thisPlayer.healthPercent < 1f)
        {
            Player.Instance.thisPlayer.TakeHealthPercent(heal, null);
            Player.Instance.buffSystem.AddBuff(testBuff.InitializeBuff(Player.Instance.gameObject));
            itemAmount--;
        }
    }
}
