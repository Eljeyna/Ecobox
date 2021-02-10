using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

public class SaveLoadSystem : MonoBehaviour
{
    public static SaveLoadSystem Instance { get; private set; }

    private string json;

    private void Awake()
    {
        if (ReferenceEquals(Instance, null))
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        if (!Directory.Exists(StaticGameVariables._SAVE_FOLDER))
        {
            Directory.CreateDirectory(StaticGameVariables._SAVE_FOLDER);
        }
    }

    public void Save()
    {
        StringBuilder sb = new StringBuilder(Path.Combine(StaticGameVariables._SAVE_FOLDER, "save0.json"));
        
        json = string.Empty;
        foreach (var monoBehaviour in FindObjectsOfType<MonoBehaviour>())
        {
            if (monoBehaviour is ISaveState persist)
            {
                json += persist.Save();
            }
        }
        
        File.WriteAllText(sb.ToString(), json);
    }

    public async void Load()
    {
        await Preload();
        
        AfterLoadSystem.Instance.Load();
    }

    public async Task Preload()
    {
        foreach (var monoBehaviour in FindObjectsOfType<MonoBehaviour>())
        {
            if (monoBehaviour is ISaveState persist)
            {
                await persist.Load();
            }
        }
    }
}

public struct Saveable
{
    public List<string> questID;
    public List<int> questTask;
    public List<QuestState> questStates;
    public List<string> completedQuestID;
    public string activeQuestID;
    
    public List<string> itemsID;
    public List<int> itemsAmount;

    public int maxStamina;
    public int stamina;
    public int staminaRegen;
    public float staminaTimeRegen;
    public int level;
    public int exp;
    public int talentPoints;
    public float weight;
    public int strength;
    public int agility;
    public int intelligence;
    public int oratory;
    public int money;

    public float maxHealth;
    public float health;
    public float healthPercent;
    public float[] resistances;
    public bool invinsibility;

    public Gun weapon;
    public float positionX;
    public float positionY;
}

internal interface ISaveState
{
    string Save();
    Task Load();
}
