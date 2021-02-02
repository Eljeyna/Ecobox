using UnityEngine;

public abstract class ScriptableObjectBuff : ScriptableObject
{
    public float duration;
    public bool isStackable;

    public abstract Buff InitializeBuff(GameObject obj);
}
