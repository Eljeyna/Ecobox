using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static StaticGameVariables;

[CreateAssetMenu(menuName = "ScriptableObjects/Items/New Item Info")]
public class ItemInfo : ScriptableObject, ITranslate
{
    public Sprite itemIcon;
    [SerializeField] private AssetReferenceAtlasedSprite atlasSprite;

    public string itemName;
    public string itemDescription;

    public async Task LoadSprite()
    {
        if (atlasSprite.IsValid())
        {
            itemIcon = (Sprite)atlasSprite.OperationHandle.Result;
            return;
        }

        AsyncOperationHandle<Sprite> asyncOperationHandle = atlasSprite.LoadAssetAsync<Sprite>();

        await asyncOperationHandle.Task;

        if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
        {
            itemIcon = asyncOperationHandle.Result;
        }
    }

    public void UnloadSprite()
    {
        if (atlasSprite.IsValid())
        {
            atlasSprite.ReleaseAsset();
        }
    }

    public void GetTranslate()
    {
        StringBuilder sb = new StringBuilder(GetAsset(Path.Combine("Localization", languageKeys[(int)language], "Items", $"{name}.json")));

#if UNITY_ANDROID
        if (sb.ToString() == string.Empty)
        {
            return;
        }
        
        ItemStruct json = JsonConvert.DeserializeObject<ItemStruct>(sb.ToString());
#else
        if (!File.Exists(sb.ToString()))
        {
            return;
        }

        ItemStruct json = JsonConvert.DeserializeObject<ItemStruct>(File.ReadAllText(sb.ToString()));
#endif
        itemName = json.itemName;
        itemDescription = json.itemDescription;
    }

    public struct ItemStruct
    {
        public string itemName;
        public string itemDescription;
    }
}
