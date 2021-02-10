using UnityEngine;

public abstract class QuestUpdate : MonoBehaviour
{
    public string id;

    public abstract void UpdateQuest();
    public abstract void UpdateQuest(int nextTask);
}
