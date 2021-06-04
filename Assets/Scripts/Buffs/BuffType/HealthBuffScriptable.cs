using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Buffs/New Health Buff")]
public class HealthBuffScriptable : ScriptableObjectBuff
{
    public int parameter;

    public override void InitializeBuff(GameObject obj)
    {
        buff = new HealthBuff(this, obj);
    }
}