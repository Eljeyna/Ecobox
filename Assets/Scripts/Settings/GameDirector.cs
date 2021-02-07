using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AddressableAssets;

public class GameDirector : MonoBehaviour
{
    public static GameDirector Instance { get; private set; }

    public QuestTasksDatabase tasks;
    public GameObject dialogues;
    public GameObject items;

    public AssetReference gameUI;
    public TMP_Text debugFPS;

    public IsTalking dialogue;
    public Quest activeQuest;

    [HideInInspector] public List<Quest> quests = new List<Quest>();
    [HideInInspector] public List<int> completedQuests = new List<int>();
    [HideInInspector] public bool controlAfter = true;

    private bool DEBUG = true;
    private float timeToUpdate;

    private void Awake()
    {
        Instance = this;

        gameUI.InstantiateAsync();
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

    public void Initialize()
    {
        for (int i = 0; i < completedQuests.Count; i++)
        {
            if (completedQuests[i] == 0)
            {
                return;
            }
        }

        activeQuest = new Quest(0, QuestState.Active);
        quests.Add(activeQuest);
        UpdateQuestDescription(activeQuest, activeQuest.currentTask);

        dialogues.SetActive(true);
        items.SetActive(true);

        Player.Instance.Initialize();
    }

    public void AddNewQuest(int id)
    {
        quests.Add(new Quest(id));
    }

    public void AddNewQuest(int id, QuestState state)
    {
        quests.Add(new Quest(id, state));
    }

    public void AddNewQuest(int id, QuestState state, int currentTask)
    {
        quests.Add(new Quest(id, state, currentTask));
    }

    public void UpdateQuestDescription(Quest quest, int currentTask)
    {
        StaticGameVariables.questName.text = quest.tasks.nameQuest[(int)StaticGameVariables.language];
        StaticGameVariables.taskDescription.text = StaticGameVariables.language == StaticGameVariables.Language.Russian ?
            quest.tasks.tasks[currentTask].description[0] :
            quest.tasks.english[currentTask].description[0];
    }

    public void UpdateQuest(int id)
    {
        Quest quest = GetQuest(id);

        if (quest != null)
        {
            quest.currentTask += 1;
        }

        UpdateQuestDescription(quest, quest.currentTask);
    }

    public void UpdateQuest(int id, int task)
    {
        Quest quest = GetQuest(id);

        if (quest != null)
        {
            quest.currentTask = task;
        }

        UpdateQuestDescription(quest, quest.currentTask);
    }

    public void UpdateQuest(Quest quest)
    {
        quest.currentTask += 1;

        UpdateQuestDescription(quest, quest.currentTask);
    }

    public void UpdateQuest(Quest quest, int task)
    {
        quest.currentTask = task;

        UpdateQuestDescription(quest, quest.currentTask);
    }

    public void CompleteQuest(int id)
    {
        for (int i = 0; i < quests.Count; i++)
        {
            if (quests[i].id == id)
            {
                completedQuests.Add(quests[i].id);
                quests.RemoveAt(i);
            }
        }
    }

    public QuestTasks GetQuestTasks(int id)
    {
        for (int i = 0; i < tasks.questTasks.Length; i++)
        {
            if (tasks.questTasks[i].id == id)
            {
                return tasks.questTasks[i];
            }
        }

        return null;
    }

    public Quest GetQuest(int id)
    {
        for (int i = 0; i < quests.Count; i++)
        {
            if (quests[i].id == id)
            {
                return quests[i];
            }
        }

        return null;
    }

    public bool DialogueAction(DialogueAction dialogueAction, int parameter)
    {
        switch(dialogueAction)
        {
            case global::DialogueAction.AddExp:
                break;
            case global::DialogueAction.AddStrength:
                break;
            case global::DialogueAction.AddAgility:
                break;
            case global::DialogueAction.AddIntelligence:
                break;
            case global::DialogueAction.AddStamina:
                break;
            case global::DialogueAction.CheckStrength:
                break;
            case global::DialogueAction.CheckAgility:
                break;
            case global::DialogueAction.CheckIntelligence:
                break;
        }

        return false;
    }

    public void StartDialogue()
    {
        StaticGameVariables.PauseGame();
        Player.Instance.cam.m_Lens.OrthographicSize = 15f;

        GameUI.Instance.dialogue_box.enabled = true;
        GameUI.Instance.circleRepeat.SetActive(true);
    }

    public void StopDialogue()
    {
        GameUI.Instance.dialogue_box.enabled = false;
        GameUI.Instance.circleRepeat.SetActive(false);

        if (controlAfter)
            StaticGameVariables.ResumeGame();
    }

    public void ShowButtons()
    {
        for (int i = 0; i < GameUI.Instance.dialogueButtons.Length; i++)
        {
            GameUI.Instance.dialogueButtons[i].gameObject.SetActive(true);
        }
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
        DEBUG = false;
    }
}
