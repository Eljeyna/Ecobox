using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class ItemDatabase
{
    public static Dictionary<int, Item> allItems = new Dictionary<int, Item>();
    public static AsyncOperationHandle<IList<Item>> handle;

    public static async Task Initialize()
    {
        handle = Addressables.LoadAssetsAsync<Item>("items", null);
        await handle.Task;
    }

    public static void OnLoad()
    {
        if (handle.Status != AsyncOperationStatus.Succeeded)
        {
            Debug.LogError("Items not loaded!");
            return;
        }
        
        foreach (var item in handle.Result)
        {
            allItems.Add(item.id, item);
        }
    }

    public static Item GetItem(int id)
    {
        if (allItems.TryGetValue(id, out Item value))
        {
            return value;
        }

        return null;
    }
}
