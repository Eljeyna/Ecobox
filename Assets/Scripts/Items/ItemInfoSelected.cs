using UnityEngine;
using UnityEngine.UI;

public class ItemInfoSelected : MonoBehaviour
{
    public Item item;

    public Image sprite;

    private void Awake()
    {
        if (TryGetComponent(out Image newSprite))
        {
            sprite = newSprite;
        }
    }

    public void GetItemInfo()
    {
        if (Game.slotSelected)
        {
            if (Game.slotSelected.TryGetComponent(out Image newImage))
            {
                newImage.color = Game.slotDefaultColor;
            }
        }

        Game.slotSelected = transform.parent.gameObject;
        if (Game.slotSelected.TryGetComponent(out Image newColor))
        {
            newColor.color = Game.slotColor;
        }

        Game.itemSelected = item;
        Game.itemSelected.GetTranslate();
        Game.itemName.text = Game.translationString[0];
        Game.itemDescription.text = Game.translationString[1];

        if (!Game.itemInfoCanvas.isActiveAndEnabled)
        {
            Game.itemInfoCanvas.enabled = true;
        }

        Game.EnableInventoryButtons();

        if (item.itemType == Item.ItemType.Quest)
        {
            Game.buttonDisItem.interactable = false;
            Game.buttonDropItem.interactable = false;
        }
    }
}
