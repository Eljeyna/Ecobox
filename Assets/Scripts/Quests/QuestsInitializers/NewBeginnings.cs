using UnityEngine;
//using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "ScriptableObjects/Quests/New Quest Initializer")]
public class NewBeginnings : QuestInitialize
{
    //public AssetReference syntheaBox;
    public override void Initialize(int taskID)
    {
        if (taskID == 0)
        {
            //Addressables.InstantiateAsync(syntheaBox, StaticGameVariables._ITEMS);
        }
    }
}
