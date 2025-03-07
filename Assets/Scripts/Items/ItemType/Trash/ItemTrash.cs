﻿using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Items/ItemType/New Trash Item")]
public class ItemTrash : Item
{
    public TrashType trashType;

    public override void Use()
    {
        if (Player.Instance.trashBin)
        {
            if (Player.Instance.trashBin.trashType == trashType)
            {
                Player.Instance.trashBin.GetReward(itemAmount);
                Game.UpdateMaterialUI();
            }

            Game.DisableInventoryButtons();
            Game.itemInfoCanvas.enabled = false;
            Player.Instance.inventory.RemoveItem(Game.itemSelected);
            Game.itemSelected = null;
            Player.Instance.inventory.CallUpdateInventory();
        }
    }
}
