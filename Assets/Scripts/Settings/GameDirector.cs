using TMPro;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    public static GameDirector Instance { get; private set; }

    public Canvas dialogue_box;
    public TMP_Text dialogue_box_text;
    public TMP_Text dialogue_box_name;
    public Font dialogue_box_button_font;

    public bool controlAfter = true;

    public IsTalking dialogue;

    public DialogueButton[] dialogueButtons;
    public InvisibleButton invisibleButton;

    private void Awake()
    {
        Instance = this;
    }

    public void StartDialogue()
    {
        StaticGameVariables.PauseGame();

        dialogue_box.enabled = true;
    }

    public void StopDialogue()
    {
        dialogue_box.enabled = false;

        if (controlAfter)
            StaticGameVariables.ResumeGame();
    }

    public void ShowButtons()
    {
        for (int i = 0; i < dialogueButtons.Length; i++)
        {
            dialogueButtons[i].gameObject.SetActive(true);
        }
    }

    public void HideButtons()
    {
        for (int i = 0; i < dialogueButtons.Length; i++)
        {
            dialogueButtons[i].gameObject.SetActive(false);
        }
    }
}
