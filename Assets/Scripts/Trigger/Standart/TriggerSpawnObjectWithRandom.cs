using UnityEngine;

public class TriggerSpawnObjectWithRandom : Trigger
{
    [Range(0f, 1f)] public float chance;
    public Addressables_Instantiate enemyScript;
    public override void Use(Collider2D obj)
    {
        if (obj.TryGetComponent(out BaseTag tagEntity))
        {
            if ((tagEntity.entityTag & Tags.FL_PLAYER) != 0)
            {
                Game.GetRandom();

                if (Game.random <= chance)
                {
                    enemyScript.SpawnEntities();
                }

                if (destroyOnExecute)
                {
                    gameObject.SetActive(false);
                    Destroy(gameObject);
                }
            }
        }
    }
}
