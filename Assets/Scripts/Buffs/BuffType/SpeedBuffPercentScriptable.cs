using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Buffs/New Speed Percent Buff")]
public class SpeedBuffPercentScriptable : ScriptableObjectBuff
{
    public int parameter;

    public override Buff InitializeBuff(GameObject obj)
    {
        buff = new SpeedBuffPercent(this, obj);
        return buff;
    }
}