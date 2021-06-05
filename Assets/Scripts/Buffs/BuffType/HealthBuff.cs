using UnityEngine;

public class HealthBuff : Buff
{
    private readonly BaseCommon thisEntity;

    public HealthBuff(ScriptableObjectBuff buff, GameObject obj) : base(buff, obj)
    {
        if (obj.TryGetComponent(out BaseCommon newEntity))
        {
            thisEntity = newEntity;
        }
    }

    public override void ApplyEffect()
    {
        HealthBuffScriptable appliedBuff = (HealthBuffScriptable)buffData;
        thisEntity.SetMaxHealth(thisEntity.maxHealth + appliedBuff.parameter);
        thisEntity.EventHealthChanged();
    }

    public override void End()
    {
        HealthBuffScriptable appliedBuff = (HealthBuffScriptable)buffData;
        thisEntity.SetMaxHealth(thisEntity.maxHealth - appliedBuff.parameter * stacks);
        thisEntity.EventHealthChanged();
        stacks = 1;
        isFinished = true;
    }
}
