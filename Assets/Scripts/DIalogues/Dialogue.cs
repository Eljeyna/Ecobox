using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using static StaticGameVariables;

public enum DialogueCheckRequirements
{
    None = 0,
    Strength,
    Agility,
    Intelligence
}

public class Dialogue : MonoBehaviour, ITranslate
{
    [SerializeField] public string key;
    [SerializeField] public Sentences[] dialogues;
    [SerializeField] public AnswersArray[] answersArray;

    public void GetTranslate()
    {
        StringBuilder sb = new StringBuilder(GetAsset(Path.Combine("Localization", languageKeys[(int)language], "Dialogues", $"{key}.json")));

#if UNITY_ANDROID && !UNITY_EDITOR_LINUX
        if (sb.ToString() == string.Empty)
        {
            return;
        }
        
        Sentences[] json = JsonConvert.DeserializeObject<Sentences[]>(sb.ToString());
#else
        if (!File.Exists(sb.ToString()))
        {
            return;
        }

        Sentences[] json = JsonConvert.DeserializeObject<Sentences[]>(File.ReadAllText(sb.ToString()));
#endif
        dialogues = json;
    }
}

[System.Serializable]
public struct Sentences
{
    public string name;
    [TextArea] public string text;
    public AnswersFile[] answers;
}

[System.Serializable]
public struct AnswersArray
{
    public Answers[] answers;
}

[System.Serializable]
public struct Answers
{
    public int goto_line;
    public DialogueScript script;
    public DialogueCheckRequirements check;
    public int checkParameter;
}

public struct AnswersFile
{
    public string answer_text;
}
