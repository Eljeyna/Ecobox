using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using static StaticGameVariables;

[CreateAssetMenu(menuName = "ScriptableObjects/Items/New Item")]
public class Item : ScriptableObject
{
    public enum ItemType
    {
        Weapon,
        Cloth,
        Consumable,
        Trash,
        Quest
    }

    public enum ItemQuality
    {
        Common,
        Uncommon,
        Rare,
        Epic,
        Legendary
    }
    
    public int id;
    public AssetReference idReference;
    public string itemName;
    
    public bool itemEnd = true;
    public int itemCost;
    public int itemAmount = 1;
    
    public ItemType itemType;
    public ItemQuality itemQuality;
    
    [SerializeField] private AssetReferenceAtlasedSprite atlasSprite;

    public virtual void Use() {}
    
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

    public void GetTranslate()
    {
        StringBuilder sb = new StringBuilder(GetAsset(Path.Combine("Localization", languageKeys[(int)language], "Items", $"{itemName}.json")));

#if UNITY_ANDROID && !UNITY_EDITOR_LINUX
        if (sb.ToString() == string.Empty)
        {
            translationString[0] = string.Empty;
            translationString[1] = string.Empty;
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

        translationString[0] = json.itemName;
        translationString[1] = json.itemDescription;
    }

    private void OnDestroy()
    {
        if (idReference.IsValid())
        {
            idReference.ReleaseAsset();
        }

        UnloadSprite();
    }

    private void OnValidate()
    {
        itemName = name;
    }
}

public struct ItemStruct
{
    public string itemName;
    public string itemDescription;
}
