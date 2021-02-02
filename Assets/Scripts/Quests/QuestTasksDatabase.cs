using UnityEngine;

[CreateAssetMenu(fileName = "Tasks Database", menuName = "ScriptableObjects/Quests/Database")]
public class QuestTasksDatabase : ScriptableObject
{
    [SerializeField] public QuestTasks[] questTasks;
}
