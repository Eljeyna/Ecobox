using UnityEngine;
using UnityEngine.AddressableAssets;

public abstract class ScriptableObjectBuff : ScriptableObject
{
    public Buff buff;
    public float duration;
    public bool isStackable;
    public bool isPersist;
    
    public AssetReference idReference;

    public abstract Buff InitializeBuff(GameObject obj);

    public Buff GetBuff()
    {
        return buff;
    }

    private void OnDestroy()
    {
        idReference.ReleaseAsset();
    }
}
