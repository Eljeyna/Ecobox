using UnityEngine;

public class ItemsLoad : MonoBehaviour
{
    public void Initialize()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent(out ItemWorld itemWorld))
            {
                itemWorld.enabled = true;
            }
        }
    }
}
