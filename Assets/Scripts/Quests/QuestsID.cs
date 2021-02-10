using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;

public class QuestsID
{
    public static readonly string questsIDPath = Path.Combine("QuestsID", "QuestsID.json");
    
    public static string GetQuestID(string key)
    {
        StringBuilder sb = new StringBuilder(StaticGameVariables.GetAsset(questsIDPath));
        
#if UNITY_ANDROID
        QuestIDStruct json = JsonConvert.DeserializeObject<QuestIDStruct>(sb.ToString());
#else
        if (!File.Exists(sb.ToString()))
        {
            return string.Empty;
        }

        QuestIDStruct json = JsonConvert.DeserializeObject<QuestIDStruct>(File.ReadAllText(sb.ToString()));
#endif
        return json.questsID[key];
    }
}

public struct QuestIDStruct
{
    public Dictionary<string, string> questsID;
}