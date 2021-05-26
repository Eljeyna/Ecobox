using UnityEngine;

public class TriggerSpawnEnemyWithRandom : Trigger
{
    [Range(0f, 1f)] public float chance;
    public Addressables_Instantiate enemyScript;
    public override void Use()
    {
        StaticGameVariables.GetRandom();
        
        if (StaticGameVariables.random <= chance)
        {
            enemyScript.SpawnEntities();
        }

        if (destroyOnExit)
        {
            Destroy(gameObject);
        }
    }
}
