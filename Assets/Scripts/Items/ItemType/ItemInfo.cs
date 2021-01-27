using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

[CreateAssetMenu(fileName = "ItemInfo", menuName = "ScriptableObjects/Items/ItemInfo")]
public class ItemInfo : ScriptableObject
{
    public Sprite itemIcon;
    public string[] itemName;
    public string[] itemDescription;

    [SerializeField] private AssetReferenceAtlasedSprite atlasSprite;

    public async Task LoadSprite()
    {
        if (atlasSprite.IsValid())
        {
            itemIcon = (Sprite)atlasSprite.OperationHandle.Result;
            return;
        }

        AsyncOperationHandle<Sprite> asyncOperationHandle = atlasSprite.LoadAssetAsync<Sprite>();

        await asyncOperationHandle.Task;

        switch (asyncOperationHandle.Status)
        {
            case AsyncOperationStatus.Succeeded:
                itemIcon = asyncOperationHandle.Result;
                break;
            default:
                break;
        }
    }

    public void UnloadSprite()
    {
        if (!atlasSprite.IsValid())
        {
            return;
        }

        atlasSprite.ReleaseAsset();
    }
}
