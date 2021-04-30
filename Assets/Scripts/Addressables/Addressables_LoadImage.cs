using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class Addressables_LoadImage : MonoBehaviour
{
    public AssetReferenceAtlasedSprite atlasSprite;

    private void Awake()
    {
        if (atlasSprite == null || atlasSprite.IsValid())
        {
            return;
        }

        GetImage();
    }

    private async void GetImage()
    {
        AsyncOperationHandle<Sprite> asyncOperationHandle = atlasSprite.LoadAssetAsync<Sprite>();

        await asyncOperationHandle.Task;

        if (asyncOperationHandle.IsValid() && asyncOperationHandle.Status == AsyncOperationStatus.Succeeded && TryGetComponent(out Image image))
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
