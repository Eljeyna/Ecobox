using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance { get; private set; }

    public Canvas dialogue_box;
    public TMP_Text dialogue_box_text;
    public TMP_Text dialogue_box_name;
    public Font dialogue_box_button_font;
    public DialogueButton[] dialogueButtons;
    public InvisibleButton invisibleButton;

    private void Awake()
    {
        Instance = this;

        StaticGameVariables.InitializeLanguage();
        StaticGameVariables.InitializeAwake();

        GameDirector.Instance.Initialize();
    }
}
