using System.IO;
using UnityEngine;
using System.Collections.Generic;

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
        json = string.Empty;
        foreach (var monoBehaviour in FindObjectsOfType<MonoBehaviour>())
        {
            if (monoBehaviour is ISaveState persist)
            {
                json += persist.Save();
            }
        }

        File.WriteAllText(StaticGameVariables._SAVE_FOLDER + "/save0.json", json);
    }

    public void Load()
    {
        foreach (var monoBehaviour in FindObjectsOfType<MonoBehaviour>())
        {
            if (monoBehaviour is ISaveState persist)
            {
                persist.Load();
            }
        }
    }
}

public struct Saveable
{
    public List<int> itemsID;
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
    void Load();
}
