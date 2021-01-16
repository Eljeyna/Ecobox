using TMPro;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    public bool canControl = true;

    public Canvas dialogue_box;
    public TMP_Text dialogue_box_text;
    public TMP_Text dialogue_box_name;
    public Font dialogue_box_button_font;

    public GUIStyle dialogueButton;
    public GUIStyle invisibleButton;

    private GameObject speaker;
    private GameObject talk;
    private bool dialogue_started = false;
    private bool controlAfter = true;

    private void Start()
    {
        dialogueButton.fontSize = Screen.height * 28 / 1080;
        invisibleButton.fontSize = dialogueButton.fontSize;
    }

    public void StartDialogue()
    {
        StaticGameVariables.inventoryCanvas.enabled = false;
        canControl = false;
        if (talk != null)
            talk.SetActive(false);
        speaker.GetComponent<IsTalking>().dialogueStart = true;
        dialogue_started = true;
        dialogue_box.enabled = true;
    }

    public void StopDialogue()
    {
        dialogue_started = false;
        dialogue_box.enabled = false;
        if (talk != null)
            talk.SetActive(true);
        if (controlAfter)
            canControl = true;
    }

    public void SetSpeaker(GameObject speaker, GameObject talk)
    {
        this.speaker = speaker;
        this.talk = talk;
    }

    public void SetControlAfter(bool controlAfter)
    {
        this.controlAfter = controlAfter;
    }

    public bool IsDialogueStarted()
    {
        return this.dialogue_started;
    }
}
