using UnityEngine;

public class TriggerSpawnEnemyWithRandom : Trigger
{
    [Range(0f, 1f)] public float chance;
    public Addressables_Instantiate enemyScript;
    public override void Use()
    {
        Game.GetRandom();
        
        if (Game.random <= chance)
        {
            enemyScript.SpawnEntities();
        }

        if (destroyOnExecute)
        {
            Destroy(gameObject);
        }
    }
}
