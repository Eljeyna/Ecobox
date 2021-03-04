using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using TMPro;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameDirector : MonoBehaviour
{
    public static GameDirector Instance { get; private set; }

    public bool noControl;

    public AssetReference gameUI;
    public TMP_Text debugFPS;

    public IsTalking dialogue;
    public Quest activeQuest;

    public AssetReference startDialogue;

    public Dictionary<string, Quest> quests = new Dictionary<string, Quest>();
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

    public void Initialize()
    {
        Player.Instance.Initialize();

        AddNewQuest("New Beginnings");
        
        UpdateQuestDescription();

        InitializeDialogue(startDialogue);
    }

    public void AddNewQuest(string id)
    {
        quests.Add(id, new Quest(id));
        CheckActiveQuest(id);
    }

    public void AddNewQuest(string id, int currentTask)
    {
        quests.Add(id, new Quest(id, currentTask));
        CheckActiveQuest(id);
    }

    public void CheckActiveQuest(string id)
    {
        if (ReferenceEquals(activeQuest, null))
        {
            activeQuest = quests[id];
        }
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
            value.Complete();
            quests.Remove(id);
            
            StringBuilder sb = new StringBuilder(Path.Combine(StaticGameVariables._SAVE_FOLDER, "cplQ.json"));
            CompletedQuestsID cplQ;

            if (File.Exists(sb.ToString()))
            {
                cplQ = JsonConvert.DeserializeObject<CompletedQuestsID>(File.ReadAllText(sb.ToString()));
            }
            else
            {
                cplQ = new CompletedQuestsID {completedQuestsID = new Dictionary<string, int>()};
            }

            if (!cplQ.completedQuestsID.TryGetValue(id, out int unused))
            {
                cplQ.completedQuestsID.Add(id, 0);
            }
            
            File.WriteAllText(sb.ToString(), JsonConvert.SerializeObject(cplQ));
        }
    }

    public Quest GetQuest(string id)
    {
        if (quests.TryGetValue(id, out Quest value))
        {
            return value;
        }

        return null;
    }

    public void StartDialogue()
    {
        noControl = true;
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
            noControl = false;
        }
    }

    public async void InitializeDialogue(AssetReference dialogueReference)
    {
        StaticGameVariables.PauseGame();
        noControl = true;
        
        if (!ReferenceEquals(this.dialogueReference, null) && this.dialogueReference.IsValid())
        {
            this.dialogueReference.ReleaseAsset();
        }
        
        this.dialogueReference = dialogueReference;
        await LoadDialogue();
    }
    
    public async Task LoadDialogue()
    {
        dialogueHandle = Addressables.InstantiateAsync(dialogueReference, StaticGameVariables._DIALOGUES);
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
        if (quests.Count > 0)
        {
            foreach (string key in quests.Keys)
            {
                quests[key].Complete();
            }
        }
        
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

    private void OnApplicationQuit()
    {
        if (Directory.Exists(StaticGameVariables._SAVE_FOLDER))
        {
            StringBuilder sb = new StringBuilder(Path.Combine(StaticGameVariables._SAVE_FOLDER, "cplQ.json"));

            if (File.Exists(sb.ToString()))
            {
                File.Delete(sb.ToString());
            }
        }
    }
}