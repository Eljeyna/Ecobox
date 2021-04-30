using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class InventoryDrop : MonoBehaviour
{
    public AssetReference[] itemsForDrop;
    [Range(0f, 1f)] public float[] chanceDrop;

    private Vector3 position;

    public void Drop()
    {
        position = transform.position;
        
        for (int i = 0; i < itemsForDrop.Length; i++)
        {
            if (StaticGameVariables.InRandom(chanceDrop[i]))
            {
                Addressables.InstantiateAsync(itemsForDrop[i], StaticGameVariables._ITEMS).Completed += SetItemPosition;
            }
        }
    }

    private void SetItemPosition(AsyncOperationHandle<GameObject> obj)
    {
        if (!GameDirector.Instance)
        {
            return;
        }
        
        if (obj.IsValid() && obj.Status == AsyncOperationStatus.Succeeded)
        {
            obj.Result.transform.position = position;
        }
    }
}
