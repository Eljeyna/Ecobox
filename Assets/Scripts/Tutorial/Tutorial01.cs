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
    private int material;
    private bool first;
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
            GameDirector.Instance.UpdateQuest("New Beginnings", 4);
            Game.sceneToSave = SceneLoading.Instance.biomes[Game.currentBiome] + " " + Game.GetNextScene();
            SaveLoadSystem.Instance.Save();
            SceneLoading.Instance.SwitchToScene(Game.sceneToSave, SceneLoading.Instance.startAnimationID);
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
                    triggerUpgrade.SetActive(true);
                }
                
                waitTime = Time.time + 1f;

                break;
            case 6:
                waitTime = Time.time + 1f;
                
                break;
            case 7:
                waitTime = Time.time + 1f;

                if (!first)
                {
                    first = true;
                    material = Player.Instance.stats.qualitativeMaterial;
                }

                if (material != Player.Instance.stats.qualitativeMaterial)
                {
                    GameDirector.Instance.UpdateQuest(questID, 8);
                    Player.Instance.stats.qualitativeMaterial += 10;
                    Player.Instance.stats.badQualityMaterial += 45;
                }

                break;
            case 8:
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
