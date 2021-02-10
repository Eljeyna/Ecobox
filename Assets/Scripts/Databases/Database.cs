using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public static class Database
{
    public static async Task<T> GetItem<T>(string id)
    {
        AsyncOperationHandle<T> handle = Addressables.LoadAssetAsync<T>(id);
        await handle.Task;
        
        return LoadItem(handle);
    }

    public static T LoadItem<T>(AsyncOperationHandle<T> handle)
    {
        if (handle.IsValid() && handle.Status == AsyncOperationStatus.Succeeded)
        {
            return handle.Task.Result;
        }

        return default;
    }
}
