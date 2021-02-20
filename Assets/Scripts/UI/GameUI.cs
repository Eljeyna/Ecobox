using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance { get; private set; }

    public Canvas dialogueBox;
    public TMP_Text dialogueBoxText;
    public TMP_Text dialogueBoxName;
    public DialogueButton[] dialogueButtons;
    public InvisibleButton invisibleButton;
    public GameObject circleRepeat;
    public Joystick joystick;

    private void Awake()
    {
        Instance = this;

        StaticGameVariables.InitializeLanguage();
        StaticGameVariables.InitializeAwake();
        StaticGameVariables.InitializeFinale();
    }
}
