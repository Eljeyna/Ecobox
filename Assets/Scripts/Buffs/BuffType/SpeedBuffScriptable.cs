using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Buffs/New Speed Buff")]
public class SpeedBuffScriptable : ScriptableObjectBuff
{
    public int parameter;

    public override void InitializeBuff(GameObject obj)
    {
        buff = new SpeedBuff(this, obj);
    }
}