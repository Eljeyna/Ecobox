using UnityEngine;

public class TriggerBuff : Trigger
{
    public ScriptableObjectBuff buff;

    public override void Use(Collider2D obj)
    {
        base.Use(obj);

        if (obj.TryGetComponent(out BaseTag tagEntity))
        {
            if ((tagEntity.entityTag & Tags.FL_PLAYER) != 0)
            {
                buff.InitializeBuff(obj.gameObject);

                if (destroyOnExecute)
                {
                    gameObject.SetActive(false);
                    Destroy(gameObject);
                }
            }
        }
    }
}
