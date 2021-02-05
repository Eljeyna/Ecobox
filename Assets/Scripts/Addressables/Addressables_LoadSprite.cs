using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Addressables_LoadSprite : MonoBehaviour
{
    public AssetReferenceAtlasedSprite atlasSprite;

    private async Task Awake()
    {
        if (atlasSprite == null)
        {
            return;
        }
        
        var image = GetComponent<SpriteRenderer>();
        AsyncOperationHandle<Sprite> asyncOperationHandle = atlasSprite.LoadAssetAsync<Sprite>();
        
        await asyncOperationHandle.Task;

        switch (asyncOperationHandle.Status)
        {
            case AsyncOperationStatus.Succeeded:
                image.sprite = asyncOperationHandle.Result;
                break;
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
