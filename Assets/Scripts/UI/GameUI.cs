using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{
    public static GameUI Instance { get; private set; }

    public Canvas dialogue_box;
    public TMP_Text dialogue_box_text;
    public TMP_Text dialogue_box_name;
    public DialogueButton[] dialogueButtons;
    public InvisibleButton invisibleButton;
    public GameObject circleRepeat;

    private void Awake()
    {
        Instance = this;

        StaticGameVariables.InitializeLanguage();
        StaticGameVariables.InitializeAwake();

        GameDirector.Instance.Initialize();
        
        if (transform.GetChild(1).TryGetComponent(out Canvas canvas))
        {
            canvas.worldCamera = Player.Instance.mainCamera;
        }
    }
}
