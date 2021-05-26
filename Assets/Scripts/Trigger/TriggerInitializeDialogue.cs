using UnityEngine.AddressableAssets;

public class TriggerInitializeDialogue : Trigger
{
    public AssetReference dialogue;
    public bool destroyOnExit;
    
    public override void Use()
    {
        GameDirector.Instance.InitializeDialogue(dialogue);

        if (destroyOnExit)
        {
            Destroy(gameObject);
        }
    }
}
