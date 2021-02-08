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
        if (StaticGameVariables.slotSelected)
        {
            if (StaticGameVariables.slotSelected.TryGetComponent(out Image newImage))
            {
                newImage.color = StaticGameVariables.slotDefaultColor;
            }
        }

        StaticGameVariables.slotSelected = transform.parent.gameObject;
        if (StaticGameVariables.slotSelected.TryGetComponent(out Image newColor))
        {
            newColor.color = StaticGameVariables.slotColor;
        }

        StaticGameVariables.itemSelected = item;
        StaticGameVariables.itemSelected.itemInfo.GetTranslate();
        StaticGameVariables.itemName.text = item.itemInfo.itemName;
        StaticGameVariables.itemDescription.text = item.itemInfo.itemDescription;

        if (!StaticGameVariables.itemInfoCanvas.isActiveAndEnabled)
        {
            StaticGameVariables.itemInfoCanvas.enabled = true;
        }

        StaticGameVariables.EnableInventoryButtons();

        if (item.itemType == Item.ItemType.Quest)
        {
            StaticGameVariables.buttonDisItem.interactable = false;
            StaticGameVariables.buttonDropItem.interactable = false;
        }
        else if (item.itemType == Item.ItemType.Trash)
        {
            StaticGameVariables.buttonUseItem.interactable = false;
        }
    }
}
