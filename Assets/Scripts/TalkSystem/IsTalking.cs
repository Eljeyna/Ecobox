using UnityEngine;

public class IsTalking : MonoBehaviour
{
    public Dialogue dialogue;
    public int _currentLine;
    public int _defaultLine;
    public bool dialogueStart = false;
    public bool controlAfter = true;

    public DialogueAction actionAfterDialogue;
    public int parameterAfterDialogue;

    private void Start()
    {
        if (dialogueStart)
        {
            StartTalk();
        }
    }

    public void StartTalk()
    {
        if (!GameUI.Instance.dialogue_box.isActiveAndEnabled)
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
        dialogueStart = false;

        if (actionAfterDialogue != 0)
        {
            GameDirector.Instance.DialogueAction(actionAfterDialogue, parameterAfterDialogue);
            return;
        }

        GameDirector.Instance.controlAfter = controlAfter;
        GameDirector.Instance.StopDialogue();

        StaticGameVariables.ShowInGameUI();
        StaticGameVariables.ResumeGame();
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
        if (dialogue.dialogues[_currentLine].answers[id].action != 0)
        {
            GameDirector.Instance.DialogueAction(dialogue.dialogues[_currentLine].answers[id].action, dialogue.dialogues[_currentLine].answers[id].parameter);
        }

        SetDialogue(dialogueLine);
    }

    public void UpdateDialogue()
    {
        if (StaticGameVariables.language == StaticGameVariables.Language.Russian)
        {
            GameUI.Instance.dialogue_box_name.text = dialogue.dialogues[_currentLine].name;
            GameUI.Instance.dialogue_box_text.text = dialogue.dialogues[_currentLine].text;
        }
        else
        {
            GameUI.Instance.dialogue_box_name.text = dialogue.english[_currentLine].name;
            GameUI.Instance.dialogue_box_text.text = dialogue.english[_currentLine].text;
        }

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
                GameUI.Instance.dialogueButtons[i + length].text.text = StaticGameVariables.language == StaticGameVariables.Language.Russian ?
                                dialogue.dialogues[_currentLine].answers[i].answer_text :
                                dialogue.english[_currentLine].answers[i].answer_text;
                GameUI.Instance.dialogueButtons[i + length].line = dialogue.dialogues[_currentLine].answers[i].goto_line;
            }
        }
        else
        {
            GameDirector.Instance.HideButtons();
            GameUI.Instance.invisibleButton.gameObject.SetActive(true);
        }
    }
}
