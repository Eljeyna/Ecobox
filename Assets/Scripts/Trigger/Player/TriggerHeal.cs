using UnityEngine;

public class TriggerHeal : Trigger
{
    public float amount;

    public override void Use(Collider2D obj)
    {
        base.Use(obj);

        if (obj.TryGetComponent(out BaseTag tagEntity) && obj.TryGetComponent(out BaseCommon baseCommon))
        {
            if ((tagEntity.entityTag & Tags.FL_PLAYER) != 0)
            {
                baseCommon.TakeHealth(amount, null);

                if (destroyOnExecute)
                {
                    gameObject.SetActive(false);
                    Destroy(gameObject);
                }
            }
        }
    }
}
