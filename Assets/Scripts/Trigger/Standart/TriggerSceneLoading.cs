using UnityEngine;

public class TriggerSceneLoading : Trigger
{
    public string scene;

    public override void Use(Collider2D obj)
    {
        base.Use(obj);

        if (obj.TryGetComponent(out BaseTag tagEntity))
        {
            if ((tagEntity.entityTag & Tags.FL_PLAYER) != 0)
            {
                SceneLoading.Instance.SwitchToScene(scene, SceneLoading.Instance.startAnimationID);
            }
        }
    }
}
