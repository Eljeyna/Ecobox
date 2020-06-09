using UnityEditor;
using UnityEngine;

public class IsTalking : MonoBehaviour
{
    public GameDirector game;
    public DialogueNode[] node;
    public string speakerName;
    public int _currentNode;
    public bool isTalking = true;
    public bool dialogueStart = false;
    public bool controlAfter = true;
    public GameObject talkButton;
    
    void OnGUI()
    {
        if (dialogueStart)
        {
            if (!game.dialogue_box.activeSelf)
            {
                game.SetSpeaker(gameObject, null);
                game.StartDialogue();
                return;
            }
            game.dialogue_box_text.text = node[_currentNode].NpcText;
            game.dialogue_box_name.text = speakerName;

            if (node[_currentNode].PlayerAnswer.Length > 0)
            {
                for (int i = 0; i < node[_currentNode].PlayerAnswer.Length; i++)
                {
                    if (GUI.Button(new Rect(
                            Screen.width * 0.5f - (Screen.width * 0.15f),
                            Screen.height - (Screen.height * 0.235f) + (Screen.height * 0.11f) * i,
                            Screen.width * 0.3f,
                            Screen.height * 0.1f),
                            node[_currentNode].PlayerAnswer[i].Text,
                            game.dialogueButton)
                        )
                    {
                        if (node[_currentNode].PlayerAnswer[i].SpeakEnd)
                        {
                            if (node[_currentNode].Script != null)
                            {
                                node[_currentNode].Script.enabled = true;
                            }
                            _currentNode = 0;
                            dialogueStart = false;
                            game.SetControlAfter(controlAfter);
                            game.StopDialogue();
                        }
                        else
                        {
                            if (node[_currentNode].Script != null)
                            {
                                node[_currentNode].Script.enabled = true;
                            }
                            _currentNode = node[_currentNode].PlayerAnswer[i].ToNode;
                        }
                    }
                }
            }
            else
            {
                if (GUI.Button(new Rect(
                        0,
                        0,
                        Screen.width,
                        Screen.height),
                        "",
                        game.invisibleButton)
                    )
                {
                    if (node.Length - 1 == _currentNode)
                    {
                        if (node[_currentNode].Script != null)
                        {
                            node[_currentNode].Script.enabled = true;
                        }
                        _currentNode = 0;
                        dialogueStart = false;
                        game.SetControlAfter(controlAfter);
                        game.StopDialogue();
                    }
                    else
                    {
                        if (node[_currentNode].Script != null)
                        {
                            node[_currentNode].Script.enabled = true;
                        }
                        _currentNode = _currentNode + 1;
                    }
                }
            }
        }
    }
}

[System.Serializable]
public class DialogueNode
{
    [TextArea(3, 10)]
    public string NpcText;
    public Answer[] PlayerAnswer;
    public MonoBehaviour Script;
}


[System.Serializable]
public class Answer
{
    [TextArea(3, 10)]
    public string Text;
    public int ToNode;
    public bool SpeakEnd;
}