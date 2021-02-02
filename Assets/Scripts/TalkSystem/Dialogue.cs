using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogues")]
public class Dialogue : ScriptableObject
{
    [SerializeField] public Sentences[] dialogues;
    [SerializeField] public SentencesOther[] english;
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
    public DialogueAction action;
    public int parameter;
}



[System.Serializable]
public struct SentencesOther
{
    public string name;
    [TextArea] public string text;
    public AnswersOther[] answers;
}

[System.Serializable]
public struct AnswersOther
{
    [TextArea] public string answer_text;
}