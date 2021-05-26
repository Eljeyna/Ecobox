using UnityEngine.AddressableAssets;

public class TriggerInitializeDialogue : Trigger
{
    public AssetReference dialogue;
    
    public override void Use()
    {
        GameDirector.Instance.InitializeDialogue(dialogue);

        if (destroyOnExit)
        {
            Destroy(gameObject);
        }
    }
}
