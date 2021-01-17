using UnityEngine;

public class ItemWorld : MonoBehaviour
{
    public Item item;
    public SpriteRenderer minimapSprite;

    private void Start()
    {
        minimapSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        minimapSprite.color = StaticGameVariables.colorItems[(int)item.itemQuality];
    }
}
