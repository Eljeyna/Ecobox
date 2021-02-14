using UnityEngine;
using UnityEngine.AddressableAssets;

public class ItemWorld : MonoBehaviour
{
    public Item item;

    private void OnDestroy()
    {
        if (gameObject)
        {
            Addressables.ReleaseInstance(gameObject);
        }
    }
}
