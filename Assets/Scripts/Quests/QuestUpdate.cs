using UnityEngine;

public abstract class QuestUpdate : MonoBehaviour
{
    public int id;
    public Quest quest;

    public abstract void UpdateQuest();
    public abstract void UpdateQuest(int nextTask);
}
