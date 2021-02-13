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

    private async void Awake()
    {
        Instance = this;

        StaticGameVariables.InitializeLanguage();
        StaticGameVariables.InitializeAwake();
        
        if (transform.GetChild(1).TryGetComponent(out Canvas canvas))
        {
            canvas.worldCamera = Player.Instance.mainCamera;
        }

        if (Settings.Instance.gameIsLoaded)
        {
            Player.Instance.Initialize();
            
            await SaveLoadSystem.Instance.Load();
            
            Settings.Instance.gameIsLoaded = false;
            StaticGameVariables.ResumeGame();
        }
        else
        {
            GameDirector.Instance.Initialize();
        }
    }
}
