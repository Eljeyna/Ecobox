using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameDirector : MonoBehaviour
{
    public static GameDirector Instance { get; private set; }

    public ItemsLoad items;

    public AssetReference gameUI;
    public TMP_Text debugFPS;

    public IsTalking dialogue;
    public Quest activeQuest;

    public AssetReference testDialogue;

    public Dictionary<string, Quest> quests = new Dictionary<string, Quest>();
    [HideInInspector] public List<string> completedQuestsID = new List<string>();
    [HideInInspector] public bool controlAfter = true;

    private bool DEBUG = true;
    private float timeToUpdate;

    private AsyncOperationHandle<GameObject> dialogueHandle;
    private AssetReference dialogueReference;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!DEBUG || timeToUpdate > Time.time)
        {
            return;
        }
        
        debugFPS.text = $"{(int)(1f / Time.unscaledDeltaTime)}";
        timeToUpdate = Time.time + 1f;
    }

    public void Preload()
    {
        gameUI.InstantiateAsync();
    }

    public async void Initialize()
    {
        await LoadQuest(QuestsID.GetQuestID("New beginnings"));

        UpdateQuestDescription();
        
        Player.Instance.Initialize();

        items.Initialize();

        InitializeDialogue(testDialogue);
    }
    
    public async Task LoadQuest(string id)
    {
        activeQuest = new Quest(id, QuestState.Active);
        quests.Add(activeQuest.id, activeQuest);
        await activeQuest.Initialize();
    }

    public void AddNewQuest(string id)
    {
        quests.Add(id, new Quest(id));
    }

    public void AddNewQuest(string id, QuestState state)
    {
        quests.Add(id, new Quest(id, state));
    }

    public void AddNewQuest(string id, QuestState state, int currentTask)
    {
        quests.Add(id, new Quest(id, state, currentTask));
    }

    public void UpdateQuestDescription()
    {
        StringBuilder sb = new StringBuilder();
        activeQuest.tasks.GetTranslate();
        StaticGameVariables.questName.text = activeQuest.tasks.nameQuest;
        
        for (int i = 0; i < activeQuest.tasks.tasksDescriptions[activeQuest.currentTask].description.Length; i++)
        {
            sb.Append(activeQuest.tasks.tasksDescriptions[activeQuest.currentTask].description[i]);
        }
        
        StaticGameVariables.taskDescription.text = sb.ToString();
    }

    public void UpdateQuest(string id)
    {
        Quest quest = GetQuest(id);

        if (quest == null)
        {
            return;
        }
        
        quest.currentTask += 1;

        if (quest.id == activeQuest.id)
        {
            UpdateQuestDescription();
        }
    }

    public void UpdateQuest(string id, int task)
    {
        Quest quest = GetQuest(id);

        if (quest == null)
        {
            return;
        }
        
        quest.currentTask = task;

        if (quest.id == activeQuest.id)
        {
            UpdateQuestDescription();
        }
    }

    public void CompleteQuest(string id)
    {
        if (quests.TryGetValue(id, out Quest value))
        {
            completedQuestsID.Add(id);
            quests.Remove(id);
        }
    }

    public Quest GetQuest(string id)
    {
        if (quests.TryGetValue(QuestsID.GetQuestID(id), out Quest value))
        {
            return value;
        }

        return null;
    }

    public void StartDialogue()
    {
        Player.Instance.cam.m_Lens.OrthographicSize = 15f;

        GameUI.Instance.dialogueBox.enabled = true;
        GameUI.Instance.circleRepeat.SetActive(true);
    }

    public void StopDialogue()
    {
        if (!ReferenceEquals(dialogueReference, null) && dialogueReference.IsValid())
        {
            dialogueReference.ReleaseAsset();
        }
        
        Addressables.ReleaseInstance(dialogue.gameObject);

        GameUI.Instance.dialogueBox.enabled = false;
        GameUI.Instance.circleRepeat.SetActive(false);

        if (controlAfter)
        {
            StaticGameVariables.ResumeGame();
            StaticGameVariables.ShowInGameUI();
        }
    }

    public async void InitializeDialogue(AssetReference dialogueReference)
    {
        if (!ReferenceEquals(this.dialogueReference, null) && this.dialogueReference.IsValid())
        {
            this.dialogueReference.ReleaseAsset();
        }
        
        this.dialogueReference = dialogueReference;
        await LoadDialogue();
    }
    
    public async Task LoadDialogue()
    {
        dialogueHandle = Addressables.InstantiateAsync(dialogueReference);
        await dialogueHandle.Task;
    }

    public void HideButtons()
    {
        for (int i = 0; i < GameUI.Instance.dialogueButtons.Length; i++)
        {
            GameUI.Instance.dialogueButtons[i].gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (gameUI.IsValid())
        {
            Addressables.ReleaseInstance(GameUI.Instance.gameObject);
        }
        
        if (!ReferenceEquals(dialogueReference, null) && dialogueReference.IsValid())
        {
            dialogueReference.ReleaseAsset();
        }
        
        DEBUG = false;
    }
}
