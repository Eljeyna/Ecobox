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
        AgilityBuffScriptable speedBuff = (AgilityBuffScriptable)buff;
        stats.agility += speedBuff.parameter;
    }

    public override void End()
    {
        AgilityBuffScriptable appliedBuff = (AgilityBuffScriptable)buff;
        stats.agility -= appliedBuff.parameter * stacks;
        stacks = 0;
    }
}
