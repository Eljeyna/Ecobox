using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Tutorial01 : MonoBehaviour
{
    public AssetReference dialogue;
    public GameObject triggerUpgrade;
    public string questID;
    public Addressables_Instantiate[] items;

    private float waitTime;
    private float health;
    private int stamina;
    private bool finished;

    private void Awake()
    {
        health = Player.Instance.thisEntity.maxHealth;
        stamina = Player.Instance.stats.maxStamina;
    }

    private void LateUpdate()
    {
        if (GameDirector.Instance.activeQuest == null || Game.isPause)
        {
            return;
        }
        
        if (finished)
        {
            this.enabled = false;
            Game.sceneToSave = "Tutorial 01";
            SaveLoadSystem.Instance.Save();
            SceneLoading.Instance.SwitchToScene("MainMenu", SceneLoading.startAnimationID);
        }
        
        if (waitTime > Time.time)
        {
            return;
        }

        if (GameDirector.Instance.activeQuest.currentTask < 5)
        {
            waitTime = Time.time + 1f;
            return;
        }

        switch (GameDirector.Instance.activeQuest.currentTask)
        {
            case 5:
                if (!items[0].createdObjects[0].IsValid() && !items[1].createdObjects[0].IsValid() && !items[2].createdObjects[0].IsValid())
                {
                    GameDirector.Instance.UpdateQuest(questID, 6);
                    Player.Instance.stats.qualitativeMaterial += 10;
                    Player.Instance.stats.badQualityMaterial += 45;
                    triggerUpgrade.SetActive(true);
                }

                break;
            case 6:
                if (Player.Instance.stats.maxStamina != stamina || Player.Instance.thisEntity.maxHealth != health)
                {
                    finished = true;
                    GameDirector.Instance.InitializeDialogue(dialogue);
                }
                
                waitTime = Time.time + 1f;
                
                break;
        }
    }
}
