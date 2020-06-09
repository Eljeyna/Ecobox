using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameDirector : MonoBehaviour
{
    private GameObject speaker;
    private GameObject talk;
    private bool dialogue_started = false;
    private bool controlAfter = true;
    public bool canControl = true;
    public GameObject player;
    public GameObject dialogue_box;
    public TMP_Text dialogue_box_text;
    public TMP_Text dialogue_box_name;
    public Font dialogue_box_button_font;
    public Joystick joystick;
    public GUIStyle dialogueButton;
    public GUIStyle invisibleButton;

    private void Start()
    {
        dialogueButton.fontSize = Screen.height * 32 / 1080;
        invisibleButton.fontSize = Screen.height * 32 / 1080;
    }

    public void StartDialogue()
    {
        canControl = false;
        if (talk != null)
            talk.SetActive(false);
        speaker.GetComponent<IsTalking>().dialogueStart = true;
        dialogue_started = true;
        dialogue_box.SetActive(true);
    }

    public void StopDialogue()
    {
        dialogue_started = false;
        dialogue_box.SetActive(false);
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
