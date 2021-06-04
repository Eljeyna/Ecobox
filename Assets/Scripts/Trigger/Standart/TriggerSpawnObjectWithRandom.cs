using UnityEngine;

public class TriggerSpawnObjectWithRandom : Trigger
{
    [Range(0f, 1f)] public float chance;
    public Addressables_Instantiate enemyScript;
    public override void Use(Collider2D obj)
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
