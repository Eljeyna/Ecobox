using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Addressables_Instantiate : MonoBehaviour
{
    public AssetReference[] prefabs;

    private List<GameObject> createdObjects = new List<GameObject>();

    private void Awake()
    {
        AddressablesInstantiate();
    }

    private void AddressablesInstantiate()
    {
        for (int i = 0; i < prefabs.Length; i++)
        {
            Addressables.LoadAssetAsync<GameObject>(prefabs[i]).Completed += PrefabLoaded;
        }
    }

    private void PrefabLoaded(AsyncOperationHandle<GameObject> obj)
    {
        createdObjects.Add(Instantiate(obj.Result));
    }

    private void OnDestroy()
    {
        for (int i = 0; i < prefabs.Length; i++)
        {
            if (prefabs[i].IsValid())
            {
                prefabs[i].ReleaseAsset();
            }
        }
    }
}
