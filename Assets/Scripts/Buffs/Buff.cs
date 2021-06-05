using UnityEngine;

public abstract class Buff
{
    public int stacks = 1;
    public float duration;
    public bool isFinished;
    public readonly ScriptableObjectBuff buffData;
    private readonly GameObject obj;

    public abstract void End();

    public Buff(ScriptableObjectBuff buff, GameObject obj)
    {
        this.buffData = buff;
        this.obj = obj;
    }

    public void Tick(float delta)
    {
        duration -= delta;
        
        if (duration <= 0f)
        {
            End();
        }
    }

    public void Activate()
    {
        if (buffData.isStackable)
        {
            ApplyEffect();
            stacks++;
        }

        duration = buffData.duration;
    }
    public abstract void ApplyEffect();
}
