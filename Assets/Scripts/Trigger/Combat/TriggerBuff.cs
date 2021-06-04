using UnityEngine;

public class TriggerBuff : Trigger
{
    public SpeedBuffPercentScriptable buff;

    public override void Use(Collider2D obj)
    {
        base.Use(obj);
        buff.InitializeBuff(obj.gameObject);
    }
}
