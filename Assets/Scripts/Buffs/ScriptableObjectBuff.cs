using UnityEngine;
using UnityEngine.AddressableAssets;

public abstract class ScriptableObjectBuff : ScriptableObject
{
    public Buff buff;
    public float duration;
    public bool isStackable;
    public bool isPersist;
    
    public AssetReference idReference;

    public abstract void InitializeBuff(GameObject obj);

    private void OnDestroy()
    {
        idReference.ReleaseAsset();
    }
}
