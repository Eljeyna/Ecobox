using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class TriggerOutTheBounds : Trigger
{
    public override void Use(Collider2D obj)
    {
        base.Use(obj);
        if (obj.TryGetComponent(out BaseTag tagEntity))
        {
            if ((tagEntity.entityTag & Tags.FL_PLAYER) != 0)
            {
                SceneLoading.Instance.anim.SetTrigger(SceneLoading.Instance.startAnimationID);
                GameDirector.Instance.noControl = true;
                GetBack();
            }
            else
            {
                Addressables.ReleaseInstance(obj.gameObject);
            }
        }
    }

    private async void GetBack()
    {
        await Task.Delay(1000);

        if (!Player.Instance)
        {
            return;
        }

        if (Player.Instance.thisEntity.health > 5f)
        {
            Player.Instance.thisEntity.TakeDamage(5f, -1, null);
        }
        else
        {
            Player.Instance.Standing();
            Player.Instance.stunTime = Time.time + 1f;
            Player.Instance.state = EntityState.Stun;
        }

        Player.Instance.transform.position = Game.lastCheckpointPosition;
        SceneLoading.Instance.anim.SetTrigger(SceneLoading.Instance.endAnimationID);
        GameDirector.Instance.noControl = false;
    }
}
