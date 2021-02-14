using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;

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
    }

    private void Update()
    {
        if (StaticGameVariables.isPause)
        {
            nextStaminaRegen = StaticGameVariables.WaitInPause(nextStaminaRegen);
            return;
        }
        
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
    }

    public void AddAgility(int amount)
    {
        if (agility >= StaticGameVariables.maxBonus)
        {
            return;
        }

        agility += amount;
    }

    public void AddIntelligence(int amount)
    {
        if (intelligence >= StaticGameVariables.maxBonus)
        {
            return;
        }

        intelligence += amount;
    }

    public string Save()
    {
        Saveable saveObject = new Saveable();

        if (Player.Instance.inventory.itemList.Count > 0)
        {
            saveObject.itemsID = new List<string>();
            saveObject.itemsAmount = new List<int>();
            foreach (string key in Player.Instance.inventory.itemList.Keys)
            {
                saveObject.itemsID.Add(Player.Instance.inventory.itemList[key].idReference.AssetGUID);
                saveObject.itemsAmount.Add(Player.Instance.inventory.itemList[key].itemAmount);
            }
        }
        
        if (GameDirector.Instance.quests.Count > 0)
        {
            saveObject.questID = new List<string>();
            saveObject.questTask = new List<int>();
            foreach (string key in GameDirector.Instance.quests.Keys)
            {
                saveObject.questID.Add(GameDirector.Instance.quests[key].id);
                saveObject.questTask.Add(GameDirector.Instance.quests[key].currentTask);
            }
        }

        if (Player.Instance.buffSystem.buffs.Count > 0)
        {
            saveObject.buffsID = new List<string>();
            saveObject.buffsDuration = new List<float>();
            foreach (ScriptableObjectBuff key in Player.Instance.buffSystem.buffs.Keys)
            {
                saveObject.buffsID.Add(key.idReference.AssetGUID);
                saveObject.buffsDuration.Add(key.duration);
            }
        }

        StringBuilder saveBuilder = new StringBuilder(Path.Combine(StaticGameVariables._SAVE_FOLDER, "save0.json")); 
        StringBuilder sb = new StringBuilder(Path.Combine(StaticGameVariables._SAVE_FOLDER, "cplQ.json")); // file with Completed Quests (temporary)

        if (File.Exists(saveBuilder.ToString()))
        {
            saveObject.completedQuestsID = JsonConvert.DeserializeObject<CompletedQuestsID>(File.ReadAllText(saveBuilder.ToString())).completedQuestsID;
            if (File.Exists(sb.ToString()))
            {
                CompletedQuestsID jsonQuests = JsonConvert.DeserializeObject<CompletedQuestsID>(File.ReadAllText(sb.ToString()));

                if (jsonQuests.completedQuestsID.Count > 0)
                {
                    foreach (string key in jsonQuests.completedQuestsID.Keys)
                    {
                        if (!saveObject.completedQuestsID.TryGetValue(key, out int value))
                        {
                            saveObject.completedQuestsID.Add(key, 0);
                        }
                    }

                    CompletedQuestsID zeroing = new CompletedQuestsID {completedQuestsID = new Dictionary<string, int>()};
                    File.WriteAllText(sb.ToString(), JsonConvert.SerializeObject(zeroing));
                }
            }
        }
        else if (File.Exists(sb.ToString()))
        {
            CompletedQuestsID jsonQuests = JsonConvert.DeserializeObject<CompletedQuestsID>(File.ReadAllText(sb.ToString()));

            if (jsonQuests.completedQuestsID.Count > 0)
            {
                saveObject.completedQuestsID = new Dictionary<string, int>();
                foreach (string key in jsonQuests.completedQuestsID.Keys)
                {
                    saveObject.completedQuestsID.Add(key, 0);
                }

                CompletedQuestsID zeroing = new CompletedQuestsID {completedQuestsID = new Dictionary<string, int>()};
                File.WriteAllText(sb.ToString(), JsonConvert.SerializeObject(zeroing));
            }
        }

        if (GameDirector.Instance.activeQuest != null)
        {
            saveObject.activeQuestID = GameDirector.Instance.activeQuest.id;
        }
        else
        {
            saveObject.activeQuestID = string.Empty;
        }

        if (Player.Instance.head)
        {
            saveObject.head = Player.Instance.head.itemName;
        }
        else
        {
            saveObject.head = string.Empty;
        }
        
        if (Player.Instance.torso)
        {
            saveObject.torso = Player.Instance.torso.itemName;
        }
        else
        {
            saveObject.torso = string.Empty;
        }
        
        if (Player.Instance.legs)
        {
            saveObject.legs = Player.Instance.legs.itemName;
        }
        else
        {
            saveObject.legs = string.Empty;
        }
        
        if (Player.Instance.foots)
        {
            saveObject.foots = Player.Instance.foots.itemName;
        }
        else
        {
            saveObject.foots = string.Empty;
        }
        
        if (Player.Instance.weaponItem)
        {
            saveObject.weapon = Player.Instance.weaponItem.itemName;
        }
        else
        {
            saveObject.weapon = string.Empty;
        }

        if (Player.Instance.weaponRangedItem)
        {
            saveObject.weaponRanged = Player.Instance.weaponRangedItem.itemName;
        }
        else
        {
            saveObject.weaponRanged = string.Empty;
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
        saveObject.positionX = Player.Instance.transform.position.x;
        saveObject.positionY = Player.Instance.transform.position.y;

        return JsonConvert.SerializeObject(saveObject);
    }

    public async Task Load()
    {
        StringBuilder sb = new StringBuilder(Path.Combine(StaticGameVariables._SAVE_FOLDER, "save0.json"));

        if (File.Exists(sb.ToString()))
        {
            Saveable saveObject = JsonConvert.DeserializeObject<Saveable>(File.ReadAllText(sb.ToString()));

            if (!ReferenceEquals(saveObject.itemsID, null) && saveObject.itemsID.Count > 0)
            {
                Player.Instance.inventory.ClearInventory();
                for (int i = 0; i < saveObject.itemsID.Count; i++)
                {
                    Item item = await Database.GetItem<Item>(saveObject.itemsID[i]);
                    Player.Instance.inventory.AddItem(item, saveObject.itemsAmount[i]);
                    Addressables.Release(item);
                }
            }

            if (!ReferenceEquals(saveObject.questID, null) && saveObject.questID.Count > 0)
            {
                GameDirector.Instance.quests.Clear();
                for (int i = 0; i < saveObject.questID.Count; i++)
                {
                    GameDirector.Instance.quests.Add(saveObject.questID[i], new Quest(saveObject.questID[i], saveObject.questTask[i]));
                }
            }

            if (saveObject.activeQuestID != string.Empty)
            {
                GameDirector.Instance.activeQuest = GameDirector.Instance.quests[saveObject.activeQuestID];
                GameDirector.Instance.UpdateQuestDescription();
            }
            
            if (saveObject.head != string.Empty)
            {
                Player.Instance.inventory.itemList[saveObject.head].Use();
            }
            
            if (saveObject.torso != string.Empty)
            {
                Player.Instance.inventory.itemList[saveObject.torso].Use();
            }
            
            if (saveObject.legs != string.Empty)
            {
                Player.Instance.inventory.itemList[saveObject.legs].Use();
            }
            
            if (saveObject.foots != string.Empty)
            {
                Player.Instance.inventory.itemList[saveObject.foots].Use();
            }
            
            if (saveObject.weapon != string.Empty)
            {
                Player.Instance.inventory.itemList[saveObject.weapon].Use();
            }

            if (saveObject.weaponRanged != string.Empty)
            {
                Player.Instance.inventory.itemList[saveObject.weaponRanged].Use();
            }
            
            if (!ReferenceEquals(saveObject.buffsID, null))
            {
                for (int i = 0; i < saveObject.buffsID.Count; i++)
                {
                    ScriptableObjectBuff buff = await Database.GetItem<ScriptableObjectBuff>(saveObject.buffsID[i]);
                    Player.Instance.buffSystem.AddBuff(buff.GetBuff());
                    Player.Instance.buffSystem.buffs[buff].duration = saveObject.buffsDuration[i];
                    Addressables.Release(buff);
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
            Player.Instance.transform.position = new Vector3(saveObject.positionX, saveObject.positionY, 0f);
        }
    }
}
