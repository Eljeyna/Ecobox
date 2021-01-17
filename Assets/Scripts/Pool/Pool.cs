using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Pool : MonoBehaviour
{
    public List<AssetReference> prefabs;

    private Queue<GameObject>[] availableObjects;

    public static Pool Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        availableObjects = new Queue<GameObject>[prefabs.Count];
        for (int i = 0; i < availableObjects.Length; i++)
        {
            availableObjects[i] = new Queue<GameObject>();
        }
    }

    public async Task<GameObject> GetFromPoolAsync(int index)
    {
        if (availableObjects[index].Count == 0)
        {
            await GrowPool(index);
        }

        GameObject instance = availableObjects[index].Dequeue();
        return instance;
    }

    private async Task GrowPool(int index)
    {
        var handle = Addressables.LoadAssetAsync<GameObject>(prefabs[index]);
        handle.Completed += (operation) =>
        {
            prefabs[index].InstantiateAsync(transform).Completed += (operationHandle) =>
            {
                AddToPool(index, operationHandle.Result);
            };
        };
        await handle.Task;
    }

    public void AddToPool(int index, GameObject instance)
    {
        instance.SetActive(false);
        availableObjects[index].Enqueue(instance);
    }
}
