using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Buffs/New Speed Buff")]
public class SpeedBuffScriptable : ScriptableObjectBuff
{
    public int parameter;

    public override Buff InitializeBuff(GameObject obj)
    {
        buff = new SpeedBuff(this, obj);
        return buff;
    }
}