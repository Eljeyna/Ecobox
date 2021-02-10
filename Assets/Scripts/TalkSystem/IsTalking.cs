using UnityEngine;
using UnityEngine.UI;

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
            StaticGameVariables.PauseGame();
            StaticGameVariables.HideInGameUI();

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
            return;
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
        GameUI.Instance.dialogueBoxText.text = dialogue.dialogues[_currentLine].text;

        if (dialogue.dialogues[_currentLine].answers.Length > 0)
        {
            GameUI.Instance.invisibleButton.gameObject.SetActive(false);

            int length = GameUI.Instance.dialogueButtons.Length - dialogue.dialogues[_currentLine].answers.Length;

            for (int i = 0; i < length; i++)
            {
                GameUI.Instance.dialogueButtons[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < dialogue.dialogues[_currentLine].answers.Length; i++)
            {
                GameUI.Instance.dialogueButtons[i + length].text.text = dialogue.dialogues[_currentLine].answers[i].answer_text;
                GameUI.Instance.dialogueButtons[i + length].line = dialogue.answersArray[_currentLine].answers[i].goto_line;

                if (dialogue.answersArray[_currentLine].answers[i].check == DialogueCheckRequirements.None)
                {
                    if (GameUI.Instance.dialogueButtons[i + length].TryGetComponent(out Button button))
                    {
                        button.interactable = true;
                    }
                    
                    continue;
                }

                if (
                    (dialogue.answersArray[_currentLine].answers[i].check == DialogueCheckRequirements.Strength
                    && dialogue.answersArray[_currentLine].answers[i].checkParameter > Player.Instance.stats.strength)
                    ||
                    (dialogue.answersArray[_currentLine].answers[i].check == DialogueCheckRequirements.Agility
                    && dialogue.answersArray[_currentLine].answers[i].checkParameter > Player.Instance.stats.agility)
                    ||
                    (dialogue.answersArray[_currentLine].answers[i].check == DialogueCheckRequirements.Intelligence
                    && dialogue.answersArray[_currentLine].answers[i].checkParameter > Player.Instance.stats.intelligence)
                    )
                {
                    if (GameUI.Instance.dialogueButtons[i + length].TryGetComponent(out Button button))
                    {
                        button.interactable = false;
                    }
                }
            }
        }
        else
        {
            GameDirector.Instance.HideButtons();
            GameUI.Instance.invisibleButton.gameObject.SetActive(true);
        }
    }
}
