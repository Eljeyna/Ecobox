using UnityEngine;

public abstract class Trigger : MonoBehaviour
{
    public Collider2D triggerObject;
    public bool destroyOnExecute;

    public virtual void Use(Collider2D obj)
    {
        triggerObject = obj;
    }
}
