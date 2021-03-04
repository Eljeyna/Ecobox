using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using static StaticGameVariables;

public class QuestTasks : ITranslate
{
    public string id;
    public string nameQuest;
    public string fullDescription;
    public QuestTask[] tasksDescriptions;
    public QuestInitialize questInitialize;

    public QuestTasks(string id, int taskID)
    {
        this.id = id;
        Initialize(taskID);
    }

    public async void Initialize(int taskID)
    {
        StringBuilder sb = new StringBuilder(GetAsset(Path.Combine("Quests", "QuestsID.json")));

#if UNITY_ANDROID && !UNITY_EDITOR_LINUX
        if (sb.ToString() == string.Empty)
        {
            return;
        }
        
        Dictionary<string, string> json = JsonConvert.DeserializeObject<Dictionary<string, string>>(sb.ToString());
#else
        if (!File.Exists(sb.ToString()))
        {
            return;
        }

        Dictionary<string, string> json = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(sb.ToString()));
#endif
        if (json.TryGetValue(id, out string assetGUID))
        {
            questInitialize = await Database.GetItem<QuestInitialize>(assetGUID);
            
            if (questInitialize)
            {
                questInitialize.Initialize(taskID);
            }
        }
    }

    public void GetTranslate()
    {
        StringBuilder sb = new StringBuilder(GetAsset(Path.Combine("Localization", languageKeys[(int)language], "Quests", $"{id}.json")));

#if UNITY_ANDROID && !UNITY_EDITOR_LINUX
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
