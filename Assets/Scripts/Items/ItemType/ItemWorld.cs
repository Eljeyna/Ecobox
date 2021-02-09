using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    public Item item;
    public SpriteRenderer minimapSprite;

    private void Start()
    {
        if (transform.GetChild(0).TryGetComponent(out SpriteRenderer spriteRenderer))
        {
            minimapSprite = spriteRenderer;
        }
        
        minimapSprite.color = StaticGameVariables.colorItems[(int)item.itemQuality];
    }
}
