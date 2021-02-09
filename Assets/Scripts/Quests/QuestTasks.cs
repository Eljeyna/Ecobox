using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using static StaticGameVariables;

[CreateAssetMenu(menuName = "ScriptableObjects/Quests/New Quest Task")]
public class QuestTasks : ScriptableObject, ITranslate
{
    [SerializeField] public int id;
    [SerializeField] public string nameQuest;
    [SerializeField] public string fullDescription;
    [SerializeField] public QuestTask[] tasksDescriptions;

    public void GetTranslate()
    {
        StringBuilder sb = new StringBuilder(GetAsset(Path.Combine("Localization", languageKeys[(int)language], "Quests", $"{name}.json")));

#if UNITY_ANDROID
        if (sb.ToString() == string.Empty)
        {
            return;
        }
        
        QuestsTasksStruct json = JsonConvert.DeserializeObject<QuestsTasksStruct>(sb.ToString());
#else
        if (!File.Exists(sb.ToString()))
        {
            return;
        }

        QuestsTasksStruct json = JsonConvert.DeserializeObject<QuestsTasksStruct>(File.ReadAllText(sb.ToString()));
#endif
        nameQuest = json.nameQuest;
        fullDescription = json.fullDescription;
        tasksDescriptions = json.tasksDescriptions;
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        nameQuest = string.Empty;
        fullDescription = string.Empty;
        tasksDescriptions = new QuestTask[0];
    }
#endif
}

[Serializable]
public struct QuestTask
{
    public string[] description;
}

public struct QuestsTasksStruct
{
    public string nameQuest;
    public string fullDescription;
    public QuestTask[] tasksDescriptions;
}
