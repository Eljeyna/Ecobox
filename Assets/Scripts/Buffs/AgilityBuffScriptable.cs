using UnityEngine;

[CreateAssetMenu(menuName = "Buffs/Agility Buff")]
public class AgilityBuffScriptable : ScriptableObjectBuff
{
    public int parameter;

    public override Buff InitializeBuff(GameObject obj)
    {
        return new AgilityBuff(this, obj);
    }
}