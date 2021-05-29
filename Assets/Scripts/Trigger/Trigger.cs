using UnityEngine;

public abstract class Trigger : MonoBehaviour
{
    public bool destroyOnExecute;
    public virtual void Use() {}
}
