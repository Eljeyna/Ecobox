using UnityEngine;

public abstract class Buff
{
    protected int stacks;
    protected float duration;
    public bool isPersist;
    public bool isFinished;
    public ScriptableObjectBuff buff;
    protected readonly GameObject obj;

    public abstract void End();

    public Buff(ScriptableObjectBuff buff, GameObject obj)
    {
        this.buff = buff;
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
        if (buff.isStackable || duration <= 0)
        {
            ApplyEffect();
            stacks++;
        }

        duration = buff.duration;
    }
    protected abstract void ApplyEffect();
}
