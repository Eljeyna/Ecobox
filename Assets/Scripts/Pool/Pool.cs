using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public enum PoolID
{
    Target = 0,
    
}

public class Pool : MonoBehaviour
{
    public static Pool Instance { get; private set; }

    public AssetReference[] prefabs;

    private Queue<GameObject>[] availableObjects;

    private void Awake()
    {
        Instance = this;

        availableObjects = new Queue<GameObject>[prefabs.Length];
        for (int i = 0; i < availableObjects.Length; i++)
        {
            availableObjects[i] = new Queue<GameObject>();
        }
    }

    public GameObject GetFromPoolAsync(int index)
    {
        if (availableObjects[index].Count == 0)
        {
            GrowPool(index);
        }
        
        GameObject instance = availableObjects[index].Dequeue();
        return instance;
    }

    private async void GrowPool(int index)
    {
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(prefabs[index]);
        await handle.Task;
        
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            AddToPool(index, Instantiate(handle.Result, Vector3.zero, Quaternion.identity, transform.GetChild(index)));
        }
    }

    public void AddToPool(int index, GameObject instance)
    {
        instance.SetActive(false);
        availableObjects[index].Enqueue(instance);
    }
}
