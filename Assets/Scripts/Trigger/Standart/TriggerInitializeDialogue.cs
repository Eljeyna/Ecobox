using UnityEngine;
using UnityEngine.AddressableAssets;

public class TriggerInitializeDialogue : Trigger
{
    public AssetReference dialogue;
    
    public override void Use(Collider2D obj)
    {
        if (obj.TryGetComponent(out BaseTag tagEntity))
        {
            if ((tagEntity.entityTag & Tags.FL_PLAYER) != 0)
            {
                GameDirector.Instance.InitializeDialogue(dialogue);

                if (destroyOnExecute)
                {
                    gameObject.SetActive(false);
                    Destroy(gameObject);
                }
            }
        }
    }
}
