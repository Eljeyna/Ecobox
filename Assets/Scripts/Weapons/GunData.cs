using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(menuName = "ScriptableObjects/GunData/New Gun Data")]
public class GunData : ScriptableObject
{
    public WeaponDamageType damageType;
    public float damage;
    public float radius;
    public float range;
    public float delay;
    public float lateDelay;
    public float fireRatePrimary;
    public float impactForce;

    public float reloadTime;

    public bool autoreload = true;

    public int maxClip;
    
    [SerializeField] private AssetReferenceAtlasedSprite atlasSprite;
    
    public async Task<Sprite> LoadSprite()
    {
        if (atlasSprite.IsValid())
        {
            return (Sprite)atlasSprite.OperationHandle.Result;
        }

        AsyncOperationHandle<Sprite> asyncOperationHandle = atlasSprite.LoadAssetAsync<Sprite>();

        await asyncOperationHandle.Task;

        if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
        {
            return asyncOperationHandle.Result;
        }

        return null;
    }

    public void UnloadSprite()
    {
        if (atlasSprite.IsValid())
        {
            atlasSprite.ReleaseAsset();
        }
    }
    
    private void OnDestroy()
    {
        UnloadSprite();
    }
}
