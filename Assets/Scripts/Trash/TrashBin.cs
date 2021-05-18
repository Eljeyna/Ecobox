using UnityEngine;

public class TrashBin : MonoBehaviour
{
    public TrashType trashType;
    public Canvas icon;
    public bool randomize;

    private void Awake()
    {
        if (!randomize)
        {
            return;
        }

        StaticGameVariables.GetRandom();
        int random = (int)(StaticGameVariables.random * 4f + 0.5f);
        trashType = (TrashType)random;
    }

    public void GetReward(int amount)
    {
        Player.Instance.stats.money += amount * 10;
        Player.Instance.stats.qualitativeMaterial += 3 * amount;
    }
}
