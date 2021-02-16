using UnityEngine;

public class MinimapItem : MonoBehaviour
{
    private void Awake()
    {
        if (TryGetComponent(out SpriteRenderer spriteRenderer) && transform.parent.TryGetComponent(out ItemWorld itemWorld))
        {
            spriteRenderer.color = StaticGameVariables.colorItems[(int)itemWorld.item.itemQuality];
            spriteRenderer.sortingOrder = (int)itemWorld.item.itemQuality;
        }
    }
}
