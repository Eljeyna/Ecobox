using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using static StaticGameVariables;

public class Dialogue : MonoBehaviour, ITranslate
{
    [SerializeField] public Sentences[] dialogues;

    private void Awake()
    {
        GetTranslate();
    }

    public void GetTranslate()
    {
        StringBuilder sb = new StringBuilder(Application.streamingAssetsPath + $"/Localization/{languageKeys[(int)language]}/Dialogues/{gameObject.name}.json");
        
#if UNITY_ANDROID
        if (!WaitAssetLoad(sb.ToString()))
        {
            return;
        }
#endif

        if (File.Exists(sb.ToString()))
        {
            DialogueFile json = JsonConvert.DeserializeObject<DialogueFile>(File.ReadAllText(sb.ToString()));

            for (int i = 0; i < json.dialogues.Length; i++)
            {
                dialogues[i].name = json.dialogues[i].name;
                dialogues[i].text = json.dialogues[i].text;
            
                for (int j = 0; j < json.dialogues[i].answers.Length; j++)
                {
                    dialogues[i].answers[j].answer_text = json.dialogues[i].answers[j].answer_text;
                }
            }
        }
    }
}

[System.Serializable]
public struct Sentences
{
    public string name;
    [TextArea] public string text;
    public Answers[] answers;
}

[System.Serializable]
public struct Answers
{
    [TextArea] public string answer_text;
    public int goto_line;
    public DialogueScript script;
}

public struct DialogueFile
{
    public SentencesFile[] dialogues;
}

public struct SentencesFile
{
    public string name;
    public string text;
    public AnswersFile[] answers;
}

public struct AnswersFile
{
    public string answer_text;
}
