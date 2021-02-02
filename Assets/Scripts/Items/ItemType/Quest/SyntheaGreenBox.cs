using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "ScriptableObjects/Items/Synthea (Quest Item)")]
public class SyntheaGreenBox : Item
{
    [SerializeField] public string talkID;
    public override void Use()
    {
        itemAmount--;

        GameDirector.Instance.UpdateQuest(0);

        StaticGameVariables.HideInventory();
        GameObject.Find(talkID).GetComponent<IsTalking>().StartTalk();
    }
}
