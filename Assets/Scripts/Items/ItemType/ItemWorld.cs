using UnityEngine;
using UnityEngine.AddressableAssets;

public class ItemWorld : MonoBehaviour
{
    public Item item;

    private void OnDestroy()
    {
        Addressables.ReleaseInstance(gameObject);
    }
}
