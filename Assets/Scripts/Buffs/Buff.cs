using UnityEngine;

public abstract class Buff
{
    protected int stacks;
    public float duration;
    public readonly bool isPersist;
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
        if (buffData.isStackable || duration <= 0f)
        {
            ApplyEffect();
            stacks++;
        }

        duration = buffData.duration;
    }
    protected abstract void ApplyEffect();
}
