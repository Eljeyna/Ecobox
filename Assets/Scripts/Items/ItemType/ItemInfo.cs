using UnityEngine;

[CreateAssetMenu(fileName = "ItemInfo", menuName = "ScriptableObjects/Items/ItemInfo")]
public class ItemInfo : ScriptableObject
{
    public Sprite itemIcon;
    public string[] itemName;
    public string[] itemDescription;
}
