using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Addressables_LoadSprite : MonoBehaviour
{
    public AssetReferenceAtlasedSprite atlasSprite;

    private void Awake()
    {
        if (atlasSprite == null || atlasSprite.IsValid())
        {
            return;
        }

        GetSprite();
    }

    private async void GetSprite()
    {
        AsyncOperationHandle<Sprite> asyncOperationHandle = atlasSprite.LoadAssetAsync<Sprite>();
        
        await asyncOperationHandle.Task;

        if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded && TryGetComponent(out SpriteRenderer image))
        {
            image.sprite = asyncOperationHandle.Result;
        }
    }

    private void OnDestroy()
    {
        if (atlasSprite.IsValid())
        {
            atlasSprite.ReleaseAsset();
        }
    }
}
