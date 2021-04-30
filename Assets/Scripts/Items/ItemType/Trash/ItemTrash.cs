using UnityEngine;

public enum TrashType
{
    SomeTrash = 0,
}

[CreateAssetMenu(menuName = "ScriptableObjects/Items/ItemType/New Trash Item")]
public class ItemTrash : Item
{
    public TrashType trashType;
}
