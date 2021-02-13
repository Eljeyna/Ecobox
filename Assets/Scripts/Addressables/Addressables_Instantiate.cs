using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Addressables_Instantiate : MonoBehaviour
{
    public AssetReference[] prefabs;
    public bool instantiateAwake;
    public bool parent;

    public AsyncOperationHandle<GameObject>[] createdObjects;
    
    private void Awake()
    {
        if (instantiateAwake)
        {
            SpawnEntities();
        }
    }

    public void SpawnEntities()
    {
        if (!ReferenceEquals(createdObjects, null) && createdObjects.Length > 0)
        {
            DeleteEntities();
        }
        
        createdObjects = new AsyncOperationHandle<GameObject>[prefabs.Length];

        if (parent)
        {
            for (int i = 0; i < prefabs.Length; i++)
            {
                createdObjects[i] = Addressables.InstantiateAsync(prefabs[i], transform, true);
            }
        }
        else
        {
            for (int i = 0; i < prefabs.Length; i++)
            {
                createdObjects[i] = Addressables.InstantiateAsync(prefabs[i]);
            }
        }
    }

    public void DeleteEntities()
    {
        if (createdObjects.Length > 0)
        {
            for (int i = 0; i < createdObjects.Length; i++)
            {
                Addressables.ReleaseInstance(createdObjects[i]);
            }
        }
    }
}
