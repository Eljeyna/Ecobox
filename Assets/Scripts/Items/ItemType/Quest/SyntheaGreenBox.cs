using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "ScriptableObjects/Items/Synthea (Quest Item)")]
public class SyntheaGreenBox : Item
{
    [SerializeField] public AssetReference talkID;
    public override void Use()
    {
        itemAmount--;

        GameDirector.Instance.UpdateQuest(0);

        StaticGameVariables.HideInventory();
        GameDirector.Instance.InitializeDialogue(talkID);
    }
}
