using UnityEngine;

public class AgilityBuff : Buff
{
    private readonly Stats stats;

    public AgilityBuff(ScriptableObjectBuff buff, GameObject obj) : base(buff, obj)
    {
        if (obj.TryGetComponent(out Stats newStats))
        {
            stats = newStats;
        }
    }

    protected override void ApplyEffect()
    {
        AgilityBuffScriptable appliedBuff = (AgilityBuffScriptable)buffData;
        stats.agility += appliedBuff.parameter;
    }

    public override void End()
    {
        AgilityBuffScriptable appliedBuff = (AgilityBuffScriptable)buffData;
        stats.agility -= appliedBuff.parameter * stacks;
        stacks = 0;
        isFinished = true;
    }
}
