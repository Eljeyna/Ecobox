using UnityEngine;

public class SpeedBuffPercent: Buff
{
    private readonly AIEntity entity;
    private float saveSpeed;

    public SpeedBuffPercent(ScriptableObjectBuff buff, GameObject obj) : base(buff, obj)
    {
        if (obj.TryGetComponent(out AIEntity newEntity))
        {
            entity = newEntity;
        }
    }

    public override void ApplyEffect()
    {
        SpeedBuffPercentScriptable appliedBuff = (SpeedBuffPercentScriptable)buffData;
        saveSpeed += entity.speed * appliedBuff.parameter / 100f;
        entity.speed += saveSpeed;
    }

    public override void End()
    {
        entity.speed -= saveSpeed;
        stacks = 1;
        saveSpeed = 0f;
        isFinished = true;
    }
}
