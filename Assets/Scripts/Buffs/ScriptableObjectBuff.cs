using UnityEngine;

public abstract class ScriptableObjectBuff : ScriptableObject
{
    public Buff buff;
    public float duration;
    public bool isStackable;
    public bool isPersist;

    public abstract Buff InitializeBuff(GameObject obj);

    public Buff GetBuff()
    {
        return buff;
    }
}
