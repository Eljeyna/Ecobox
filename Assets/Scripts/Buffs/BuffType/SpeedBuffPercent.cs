using UnityEngine;

public class SpeedBuffPercent: Buff
{
    private readonly AIEntity entity;

    public SpeedBuffPercent(ScriptableObjectBuff buff, GameObject obj) : base(buff, obj)
    {
        if (obj.TryGetComponent(out AIEntity newEntity))
        {
            entity = newEntity;
        }
    }

    protected override void ApplyEffect()
    {
        SpeedBuffPercentScriptable appliedBuff = (SpeedBuffPercentScriptable)buffData;
        entity.speed += entity.speed * appliedBuff.parameter / 100f;
    }

    public override void End()
    {
        SpeedBuffPercentScriptable appliedBuff = (SpeedBuffPercentScriptable)buffData;
        entity.speed -= entity.speed * appliedBuff.parameter * stacks / 100f;
        stacks = 0;
        isFinished = true;
    }
}
