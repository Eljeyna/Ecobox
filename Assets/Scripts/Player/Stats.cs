using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

public class Stats : MonoBehaviour, ISaveState
{
    public int maxStamina = 40;
    public int stamina;
    public int staminaRegen = 1;
    public float staminaTimeRegen = 0.2f;

    public int level = 1;
    public int exp;
    public int talentPoints;

    public float weight = 50f;

    [Space(10)]
    public int strength;
    public int agility;
    public int intelligence;

    [Space(10)]
    public int oratory;

    [Space(10)]
    public int money;

    public EventHandler OnStaminaChanged;

    private float nextStaminaRegen;

    public void Initialize()
    {
        if (Settings.Instance.gameIsLoaded)
        {
            return;
        }

        stamina = maxStamina;

        AddStrength(3);
        AddAgility(3);
        AddIntelligence(3);
    }

    private void Update()
    {
        if (stamina <= maxStamina && nextStaminaRegen <= Time.time)
        {
            stamina += staminaRegen;

            if (stamina > maxStamina)
            {
                stamina = maxStamina;
            }

            nextStaminaRegen = Time.time + staminaTimeRegen;
            OnStaminaChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    
    public void LevelUp()
    {
        if (level >= StaticGameVariables.maxLevel)
        {
            return;
        }

        talentPoints++;
    }

    public void AddStrength(int amount)
    {
        if (strength >= StaticGameVariables.maxBonus)
        {
            return;
        }

        strength += amount;
        AddStrengthBonus(amount);
    }

    public void AddAgility(int amount)
    {
        if (agility >= StaticGameVariables.maxBonus)
        {
            return;
        }

        agility += amount;
        AddAgilityBonus(amount);
    }

    public void AddIntelligence(int amount)
    {
        if (intelligence >= StaticGameVariables.maxBonus)
        {
            return;
        }

        intelligence += amount;
        AddIntelligenceBonus(amount);
    }

    public void AddStrengthBonus(int amount)
    {
        float heal = Player.Instance.thisEntity.healthPercent;
        weight += StaticGameVariables.weightBonus * amount;

        Player.Instance.thisEntity.SetMaxHealth(Player.Instance.thisEntity.maxHealth + (StaticGameVariables.healthBonus * amount));
        Player.Instance.thisEntity.TakeHealthPercent(heal, null);
        Player.Instance.thisEntity.resistances[0] += StaticGameVariables.resistanceAll * amount;
        Player.Instance.thisEntity.resistances[1] += StaticGameVariables.resistanceAll * amount;
    }

    public void AddAgilityBonus(int amount)
    {
        maxStamina += StaticGameVariables.staminaBonus * amount;

        Player.Instance.speed += StaticGameVariables.speedBonus * amount;
    }

    public void AddIntelligenceBonus(int amount)
    {
        oratory += StaticGameVariables.oratoryBonus * amount;

        Player.Instance.thisEntity.resistances[2] += StaticGameVariables.resistanceAll * amount;
        Player.Instance.thisEntity.resistances[3] += StaticGameVariables.resistanceAll * amount;
    }

    public string Save()
    {
        Saveable saveObject = new Saveable();

        if (Player.Instance.inventory.itemList.Count > 0)
        {
            saveObject.itemsID = new List<int>();
            saveObject.itemsAmount = new List<int>();
            for (int i = 0; i < Player.Instance.inventory.itemList.Count; i++)
            {
                saveObject.itemsID.Add(Player.Instance.inventory.itemList[i].id);
                saveObject.itemsAmount.Add(Player.Instance.inventory.itemList[i].itemAmount);
            }
        }

        saveObject.maxStamina = maxStamina;
        saveObject.stamina = stamina;
        saveObject.staminaRegen = staminaRegen;
        saveObject.staminaTimeRegen = staminaTimeRegen;
        saveObject.level = level;
        saveObject.exp = exp;
        saveObject.talentPoints = talentPoints;
        saveObject.weight = weight;
        saveObject.strength = strength;
        saveObject.agility = agility;
        saveObject.intelligence = intelligence;
        saveObject.oratory = oratory;
        saveObject.money = money;

        saveObject.maxHealth = Player.Instance.thisEntity.maxHealth;
        saveObject.health = Player.Instance.thisEntity.health;
        saveObject.healthPercent = Player.Instance.thisEntity.healthPercent;
        saveObject.resistances = Player.Instance.thisEntity.resistances;
        saveObject.invinsibility = Player.Instance.thisEntity.invinsibility;

        saveObject.weapon = Player.Instance.weapon;
        saveObject.positionX = Player.Instance.transform.position.x;
        saveObject.positionY = Player.Instance.transform.position.y;
        string json = JsonConvert.SerializeObject(saveObject);

        return json;
    }

    public void Load()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append(StaticGameVariables._SAVE_FOLDER + "/save0.json");
        
#if UNITY_ANDROID
        if (!StaticGameVariables.WaitAssetLoad(sb.ToString()))
        {
            return;
        }
#endif

        if (File.Exists(sb.ToString()))
        {
            Saveable saveObject = JsonConvert.DeserializeObject<Saveable>(File.ReadAllText(sb.ToString()));

            if (saveObject.itemsID.Count > 0)
            {
                Player.Instance.inventory.ClearInventory();
                for (int i = 0; i < saveObject.itemsID.Count; i++)
                {
                    Player.Instance.inventory.AddItem(ItemDatabase.GetItem(saveObject.itemsID[i]), saveObject.itemsAmount[i]);
                }
            }

            maxStamina = saveObject.maxStamina;
            stamina = saveObject.stamina;
            staminaRegen = saveObject.staminaRegen;
            staminaTimeRegen = saveObject.staminaTimeRegen;
            level = saveObject.level;
            exp = saveObject.exp;
            talentPoints = saveObject.talentPoints;
            weight = saveObject.weight;
            strength = saveObject.strength;
            agility = saveObject.agility;
            intelligence = saveObject.intelligence;
            oratory = saveObject.oratory;
            money = saveObject.money;

            Player.Instance.aiEntity.target = null;
            Player.Instance.thisEntity.health = saveObject.health;
            Player.Instance.thisEntity.healthPercent = saveObject.healthPercent;
            Player.Instance.thisEntity.resistances = saveObject.resistances;
            Player.Instance.thisEntity.invinsibility = saveObject.invinsibility;
            Player.Instance.thisEntity.SetMaxHealth(saveObject.maxHealth);

            Player.Instance.weapon = saveObject.weapon;
            Player.Instance.transform.position = new Vector3(saveObject.positionX, saveObject.positionY, 0f);

            Player.Instance.inventory.CallUpdateInventory();
        }
    }
}
