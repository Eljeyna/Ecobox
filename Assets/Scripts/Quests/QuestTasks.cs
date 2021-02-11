using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using static StaticGameVariables;

public class QuestTasks : ITranslate
{
    public string id;
    public string nameQuest;
    public string fullDescription;
    public QuestTask[] tasksDescriptions;

    public QuestTasks(string id)
    {
        this.id = id;
    }

    public void GetTranslate()
    {
        StringBuilder sb = new StringBuilder(GetAsset(Path.Combine("Localization", languageKeys[(int)language], "Quests", $"{id}.json")));

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
