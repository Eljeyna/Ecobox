using UnityEngine;

public class IsTalking : MonoBehaviour
{
    public GameDirector game;
    public Dialogue dialogue;
    public int _currentLine;
    public bool isTalking = true;
    public bool dialogueStart = false;
    public bool controlAfter = true;

    public string speakerName;
    public MonoBehaviour scriptAfterDialogue;

    void OnGUI()
    {
        if (dialogueStart)
        {
            if (!game.dialogue_box.isActiveAndEnabled)
            {
                StaticGameVariables.PauseGame();
                StaticGameVariables.HideInGameUI();
                game.SetSpeaker(gameObject, null);
                game.StartDialogue();
                return;
            }

            if (StaticGameVariables.language == StaticGameVariables.Language.Russian)
            {
                game.dialogue_box_name.text = dialogue.dialogues[_currentLine].name;
                game.dialogue_box_text.text = dialogue.dialogues[_currentLine].text;
            }
            else
            {
                game.dialogue_box_name.text = dialogue.english[_currentLine].name;
                game.dialogue_box_text.text = dialogue.english[_currentLine].text;
            }

            if (dialogue.dialogues[_currentLine].answers.Length > 0)
            {
                float height = 0.175f + (0.1f * dialogue.dialogues[_currentLine].answers.Length);
                for (int i = 0; i < dialogue.dialogues[_currentLine].answers.Length; i++)
                {
                    if (GUI.Button(new Rect(
                            Screen.width * 0.83f - (Screen.width * 0.1125f),
                            Screen.height - (Screen.height * height) + (Screen.height * 0.1375f) * i,
                            Screen.width * 0.225f,
                            Screen.height * 0.125f),
                            StaticGameVariables.language == StaticGameVariables.Language.Russian ?
                                dialogue.dialogues[_currentLine].answers[i].answer_text :
                                dialogue.english[_currentLine].answers[i].answer_text,
                            game.dialogueButton)
                        )
                    {
                        if (dialogue.dialogues[_currentLine].answers[i].goto_line == 0)
                        {
                            IsTalkingDone();
                            return;
                        }
                        else
                        {
                            if (dialogue.dialogues[_currentLine].answers[i].goto_line == -1)
                            {
                                SetDialogue(0);
                            }
                            else if (dialogue.dialogues[_currentLine].answers[i].goto_line > 0)
                            {
                                SetDialogue(dialogue.dialogues[_currentLine].answers[i].goto_line);
                            }
                            else
                            {
                                SetDialogue();
                            }
                            return;
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
                        string.Empty,
                        game.invisibleButton)
                    )
                {
                    if (dialogue.dialogues.Length - 1 == _currentLine)
                    {
                        IsTalkingDone();
                        return;
                    }
                    else
                    {
                        _currentLine += 1;
                    }
                }
            }
        }
    }

    public void IsTalkingDone()
    {
        _currentLine = 0;
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

        game.SetControlAfter(controlAfter);
        game.StopDialogue();

        StaticGameVariables.ShowInGameUI();
        StaticGameVariables.ResumeGame();
    }

    public void SetDialogue(Dialogue dialogue)
    {

    }

    public void SetDialogue(int dialogue)
    {
        _currentLine = dialogue;
    }

    public void SetDialogue()
    {

    }
}