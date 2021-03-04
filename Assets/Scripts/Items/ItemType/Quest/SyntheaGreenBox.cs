using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "ScriptableObjects/Items/ItemType/Quest/Synthea (Quest Item)")]
public class SyntheaGreenBox : Item
{
    [SerializeField] public AssetReference talkID;
    public int taskID;
    
    public override void Use()
    {
        itemAmount--;

        GameDirector.Instance.UpdateQuest("New Beginnings", taskID);

        StaticGameVariables.HideInventory();
        GameDirector.Instance.InitializeDialogue(talkID);
    }

    private void OnDestroy()
    {
        if (talkID.IsValid())
        {
            talkID.ReleaseAsset();
        }
    }
}
