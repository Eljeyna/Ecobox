using UnityEngine;

public abstract class QuestUpdate : MonoBehaviour
{
    public string id;
    public int taskID;

    public abstract void UpdateQuest(int nextTask);
}
