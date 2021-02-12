using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Addressables_LoadMinimapIcon : MonoBehaviour
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

        if (asyncOperationHandle.IsValid() && asyncOperationHandle.Status == AsyncOperationStatus.Succeeded && TryGetComponent(out MiniMapComponent component))
        {
            component.icon = asyncOperationHandle.Result;
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