using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Addressables_Instantiate : MonoBehaviour
{
    public AssetReference[] prefabs;
    public bool parent = false;

    private AsyncOperationHandle<GameObject>[] createdObjects;
    
    private void Awake()
    {
        AddressablesInstantiate();
    }

    public void AddressablesInstantiate()
    {
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
}
