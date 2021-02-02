using UnityEngine;

[CreateAssetMenu(fileName = "Task", menuName = "ScriptableObjects/Quests")]
public class QuestTasks : ScriptableObject
{
    [SerializeField] public int id;
    [SerializeField] public string[] nameQuest;
    [SerializeField, TextArea] public string[] fullDescription;
    [SerializeField] public QuestTask[] tasks;
    [SerializeField] public QuestTask[] english;
}

[System.Serializable]
public struct QuestTask
{
    [TextArea] public string[] description;
}
