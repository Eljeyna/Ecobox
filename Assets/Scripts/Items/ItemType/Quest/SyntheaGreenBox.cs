using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "ScriptableObjects/Items/Synthea (Quest Item)")]
public class SyntheaGreenBox : Item
{
    [SerializeField] public AssetReference talkID;
    public override void Use()
    {
        itemAmount--;

        GameDirector.Instance.UpdateQuest("New beginnings");

        StaticGameVariables.HideInventory();
        GameDirector.Instance.InitializeDialogue(talkID);
    }
}
