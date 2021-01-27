using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class Addressables_LoadImage : MonoBehaviour
{
    public AssetReferenceAtlasedSprite atlasSprite;

    private async Task Awake()
    {
        var image = GetComponent<Image>();

        AsyncOperationHandle<Sprite> asyncOperationHandle = atlasSprite.LoadAssetAsync<Sprite>();

        await asyncOperationHandle.Task;

        switch (asyncOperationHandle.Status)
        {
            case AsyncOperationStatus.Succeeded:
                image.sprite = asyncOperationHandle.Result;
                break;
            default:
                break;
        }
    }

    private void OnDestroy()
    {
        if (!atlasSprite.IsValid())
        {
            return;
        }

        atlasSprite.ReleaseAsset();
    }
}
