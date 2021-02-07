using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance { get; private set; }

    public Canvas dialogueBox;
    public TMP_Text dialogueBoxText;
    public TMP_Text dialogueBoxName;
    public DialogueButton[] dialogueButtons;
    public InvisibleButton invisibleButton;
    public GameObject circleRepeat;

    private void Awake()
    {
        Instance = this;

        StaticGameVariables.InitializeLanguage();
        StaticGameVariables.InitializeAwake();
        
        if (transform.GetChild(1).TryGetComponent(out Canvas canvas))
        {
            canvas.worldCamera = Player.Instance.mainCamera;
        }
    }
}
