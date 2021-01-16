using UnityEngine;
using UnityEngine.UI;

public class ItemInfoSelected : MonoBehaviour
{
    public ItemInstance item;

    public Image sprite;

    private void Awake()
    {
        sprite = GetComponent<Image>();
    }

    public void GetItemInfo()
    {
        if (StaticGameVariables.slotSelected)
        {
            StaticGameVariables.slotSelected.GetComponent<Image>().color = StaticGameVariables.slotDefaultColor;
        }

        StaticGameVariables.slotSelected = transform.parent.gameObject;
        StaticGameVariables.slotSelected.GetComponent<Image>().color = StaticGameVariables.slotColor;

        StaticGameVariables.itemSelected = item.itemCopy;
        StaticGameVariables.itemName.text = item.itemCopy.itemInfo.itemName[(int)StaticGameVariables.language];
        StaticGameVariables.itemDescription.text = item.itemCopy.itemInfo.itemDescription[(int)StaticGameVariables.language];

        if (!StaticGameVariables.itemInfoCanvas.isActiveAndEnabled)
        {
            StaticGameVariables.itemInfoCanvas.enabled = true;
        }

        StaticGameVariables.EnableInventoryButtons();

        if (item.itemCopy.itemType == Item.ItemType.Quest)
        {
            StaticGameVariables.buttonDisItem.interactable = false;
            StaticGameVariables.buttonDropItem.interactable = false;
        }
        else if (item.itemCopy.itemType == Item.ItemType.Trash)
        {
            StaticGameVariables.buttonUseItem.interactable = false;
        }
    }
}
