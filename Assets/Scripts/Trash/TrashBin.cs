using UnityEngine;

public class TrashBin : MonoBehaviour
{
    public TrashType trashType;
    public Item reward;

    private void Awake()
    {
        StaticGameVariables.GetRandom();
        int random = (int)(StaticGameVariables.random * 4f);
        trashType = (TrashType)random;
    }

    public void GetReward()
    {

    }
}
