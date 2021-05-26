using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class Addressables_LoadImage : MonoBehaviour
{
    public AssetReferenceAtlasedSprite atlasSprite;

    private void Awake()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (atlasSprite == null || atlasSprite.IsValid())
#else
        if (atlasSprite == null || atlasSprite.IsValid() || TryGetComponent(out DeleteMobileData deleteMobileData))
#endif
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
