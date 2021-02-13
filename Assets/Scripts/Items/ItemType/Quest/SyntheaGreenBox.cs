using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "ScriptableObjects/Items/Synthea (Quest Item)")]
public class SyntheaGreenBox : Item
{
    [SerializeField] public AssetReference talkID;
    public int taskID;
    
    public override void Use()
    {
        itemAmount--;

        GameDirector.Instance.UpdateQuest("New beginnings", taskID);

        StaticGameVariables.HideInventory();
        GameDirector.Instance.InitializeDialogue(talkID);
    }
}
