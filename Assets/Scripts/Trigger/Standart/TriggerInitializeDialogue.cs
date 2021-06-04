using UnityEngine;
using UnityEngine.AddressableAssets;

public class TriggerInitializeDialogue : Trigger
{
    public AssetReference dialogue;
    
    public override void Use(Collider2D obj)
    {
        GameDirector.Instance.InitializeDialogue(dialogue);

        if (destroyOnExecute)
        {
            Destroy(gameObject);
        }
    }
}
