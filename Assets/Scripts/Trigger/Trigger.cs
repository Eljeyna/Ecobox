using UnityEngine;

public abstract class Trigger : MonoBehaviour
{
    public bool destroyOnExit;
    public virtual void Use() {}
}
