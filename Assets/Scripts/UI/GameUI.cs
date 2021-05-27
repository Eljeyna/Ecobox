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
    public Joystick joystickMove;
    public Joystick joystickAttack;

    private void Awake()
    {
        Instance = this;

        Game.InitializeLanguage();
        Game.InitializeAwake();
        Game.InitializeFinale();
    }
}
