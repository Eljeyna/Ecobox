using UnityEngine;

public class TriggerSetCheckpointPosition : Trigger
{
    public override void Use(Collider2D obj)
    {
        base.Use(obj);

        if (obj.TryGetComponent(out BaseTag tagEntity))
        {
            if ((tagEntity.entityTag & Tags.FL_PLAYER) != 0)
            {
                Game.lastCheckpointPosition = transform.position;
            }
        }
    }
}
