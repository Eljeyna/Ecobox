using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using static StaticGameVariables;

[RequireComponent(typeof(Dialogue))]
public class IsTalking : MonoBehaviour
{
    public Dialogue dialogue;
    public int _currentLine;
    public int _defaultLine;
    public bool controlAfter = true;

    public DialogueScript scriptAfterDialogue;

    private void Awake()
    {
        dialogue.GetTranslate();
        StartTalk();
    }

    public void StartTalk()
    {
        if (!GameUI.Instance.dialogueBox.isActiveAndEnabled)
        {
            PauseGame();
            HideInGameUI();

            UpdateDialogue();

            GameDirector.Instance.dialogue = this;
            GameDirector.Instance.StartDialogue();
        }
    }

    public void IsTalkingDone()
    {
        _currentLine = _defaultLine;

        if (scriptAfterDialogue != null)
        {
            scriptAfterDialogue.Use();
        }

        GameDirector.Instance.controlAfter = controlAfter;
        GameDirector.Instance.StopDialogue();
    }

    public void SetDialogue(int dialogueLine)
    {
        if (dialogueLine == 0)
        {
            IsTalkingDone();
            return;
        }
        else if (dialogueLine == -1)
        {
            _currentLine = 0;
            UpdateDialogue();
            return;
        }

        if (dialogue.dialogues.Length - 1 == _currentLine)
        {
            IsTalkingDone();
            return;
        }

        _currentLine = dialogueLine;

        UpdateDialogue();
    }

    public void SetDialogue(int id, int dialogueLine)
    {
        if (dialogue.answersArray[_currentLine].answers[id].script != null)
        {
            dialogue.answersArray[_currentLine].answers[id].script.Use();
        }

        SetDialogue(dialogueLine);
    }

    public void UpdateDialogue()
    {
        GameUI.Instance.dialogueBoxName.text = dialogue.dialogues[_currentLine].name;
        GameUI.Instance.dialogueBoxText.text = GetStringByGender(dialogue.dialogues[_currentLine].text);

        if (dialogue.dialogues[_currentLine].answers.Length > 0)
        {
            GameUI.Instance.invisibleButton.gameObject.SetActive(false);

            int length = GameUI.Instance.dialogueButtons.Length - dialogue.answersArray[_currentLine].answers.Length;

            for (int i = 0; i < length; i++)
            {
                GameUI.Instance.dialogueButtons[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < dialogue.answersArray[_currentLine].answers.Length; i++)
            {
                GameUI.Instance.dialogueButtons[i + length].gameObject.SetActive(true);
                
                GameUI.Instance.dialogueButtons[i + length].text.text = GetStringByGender(dialogue.dialogues[_currentLine].answers[i].answer_text);
                GameUI.Instance.dialogueButtons[i + length].line = dialogue.answersArray[_currentLine].answers[i].goto_line;

                if (dialogue.answersArray[_currentLine].answers[i].check == DialogueCheckRequirements.None)
                {
                    if (GameUI.Instance.dialogueButtons[i + length].TryGetComponent(out Button buttonA))
                    {
                        buttonA.interactable = true;
                    }
                    
                    continue;
                }
                
                if (GameUI.Instance.dialogueButtons[i + length].TryGetComponent(out Button buttonB))
                {
                    buttonB.interactable = false;
                }
            }
        }
        else
        {
            GameDirector.Instance.HideButtons();
            GameUI.Instance.invisibleButton.gameObject.SetActive(true);
        }
    }
    
    public string GetStringByGender(string text)
    {
        int index = text.IndexOf(genderSymbol);
        
        if (index == -1)
        {
            return text;
        }
        
        StringBuilder sb = new StringBuilder();
        int indexSplit;
        int indexLast = -1;

        while (index != -1)
        {
            sb.Append(text.Substring(indexLast + 1, index - indexLast - 1));
            
            indexSplit = text.IndexOf(splitSymbol, index + 1);
            indexLast = text.IndexOf(genderSymbol, indexSplit + 1);

            sb.Append(Player.Instance.gender
                ? text.Substring(index + 1, indexSplit - index - 1)
                : text.Substring(indexSplit + 1, indexLast - indexSplit - 1));

            index = text.IndexOf(genderSymbol, indexLast + 1);
        }
        
        sb.Append(text.Substring(indexLast + 1, text.Length - indexLast - 1));
        
        return sb.ToString();
    }
}
