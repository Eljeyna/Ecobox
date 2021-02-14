using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public enum PoolID
{
    Target = 0,
    SimpleBullet = 1,
}

public class Pool : MonoBehaviour
{
    public static Pool Instance { get; private set; }

    public AssetReference[] prefabs;

    private Queue<GameObject>[] availableObjects;

    private async void Awake()
    {
        Instance = this;

        availableObjects = new Queue<GameObject>[prefabs.Length];
        for (int i = 0; i < availableObjects.Length; i++)
        {
            availableObjects[i] = new Queue<GameObject>();
        }

        for (int i = 0; i <= (int)PoolID.SimpleBullet; i++)
        {
            AddToPool(i, await GetFromPoolAsync(i));
        }
    }

    public async Task<GameObject> GetFromPoolAsync(int index)
    {
        if (availableObjects[index].Count == 0)
        {
            AddToPool(index, await Addressables.InstantiateAsync(prefabs[index]).Task);
        }
        
        GameObject instance = availableObjects[index].Dequeue();
        instance.SetActive(true);
        return instance;
    }

    public void AddToPool(int index, GameObject instance)
    {
        instance.transform.SetParent(transform.GetChild(index));
        instance.SetActive(false);
        availableObjects[index].Enqueue(instance);
    }
}
