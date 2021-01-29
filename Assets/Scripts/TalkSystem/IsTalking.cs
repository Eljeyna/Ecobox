using UnityEngine;
using UnityEngine.UI;

public class IsTalking : MonoBehaviour
{
    public Dialogue dialogue;
    public int _currentLine;
    public int _defaultLine;
    public bool isTalking = true;
    public bool dialogueStart = false;
    public bool controlAfter = true;

    public MonoBehaviour scriptAfterDialogue;

    private void Start()
    {
        if (dialogueStart)
        {
            if (!GameDirector.Instance.dialogue_box.isActiveAndEnabled)
            {
                StaticGameVariables.PauseGame();
                StaticGameVariables.HideInGameUI();

                UpdateDialogue();

                GameDirector.Instance.dialogue = this;
                GameDirector.Instance.StartDialogue();
                return;
            }
        }
    }

    public void IsTalkingDone()
    {
        _currentLine = _defaultLine;
        dialogueStart = false;

        if (scriptAfterDialogue != null)
        {
            if ((object)scriptAfterDialogue.GetType() == typeof(IsTalking))
            {
                IsTalking nextDialogue = (IsTalking)scriptAfterDialogue;
                SetDialogue(nextDialogue.dialogue);
                return;
            }
            else
            {
                scriptAfterDialogue.enabled = true;
            }
        }

        GameDirector.Instance.controlAfter = controlAfter;
        GameDirector.Instance.StopDialogue();

        StaticGameVariables.ShowInGameUI();
        StaticGameVariables.ResumeGame();
    }

    public void SetDialogue(Dialogue dialogue)
    {
        UpdateDialogue();
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

    public void UpdateDialogue()
    {
        if (StaticGameVariables.language == StaticGameVariables.Language.Russian)
        {
            GameDirector.Instance.dialogue_box_name.text = dialogue.dialogues[_currentLine].name;
            GameDirector.Instance.dialogue_box_text.text = dialogue.dialogues[_currentLine].text;
        }
        else
        {
            GameDirector.Instance.dialogue_box_name.text = dialogue.english[_currentLine].name;
            GameDirector.Instance.dialogue_box_text.text = dialogue.english[_currentLine].text;
        }

        if (dialogue.dialogues[_currentLine].answers.Length > 0)
        {
            GameDirector.Instance.invisibleButton.gameObject.SetActive(false);

            int length = GameDirector.Instance.dialogueButtons.Length - dialogue.dialogues[_currentLine].answers.Length;

            for (int i = 0; i < length; i++)
            {
                GameDirector.Instance.dialogueButtons[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < dialogue.dialogues[_currentLine].answers.Length; i++)
            {
                GameDirector.Instance.dialogueButtons[i + length].text.text = StaticGameVariables.language == StaticGameVariables.Language.Russian ?
                                dialogue.dialogues[_currentLine].answers[i].answer_text :
                                dialogue.english[_currentLine].answers[i].answer_text;
                GameDirector.Instance.dialogueButtons[i + length].line = dialogue.dialogues[_currentLine].answers[i].goto_line;
            }
        }
        else
        {
            GameDirector.Instance.HideButtons();
            GameDirector.Instance.invisibleButton.gameObject.SetActive(true);
        }
    }
}