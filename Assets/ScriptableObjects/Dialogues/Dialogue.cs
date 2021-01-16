using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "ScriptableObjects/Dialogues")]
public class Dialogue : ScriptableObject
{
    [SerializeField] public Sentences[] dialogues;
    [SerializeField] public SentencesOther[] english;
}

[System.Serializable]
public class Sentences
{
    public string name;
    [TextArea] public string text;
    public Answers[] answers;
}

[System.Serializable]
public class Answers
{
    [TextArea] public string answer_text;
    public int goto_line;
}



[System.Serializable]
public class SentencesOther
{
    public string name;
    [TextArea] public string text;
    public AnswersOther[] answers;
}

[System.Serializable]
public class AnswersOther
{
    [TextArea] public string answer_text;
}