using UnityEngine;

public class RadiusAttack : MonoBehaviour
{
    public static void RadiusDamage(GameObject attacker, Vector3 pos, float radius, float amount)
    {
        BaseEntity thisEntity = attacker.GetComponent<BaseEntity>();

        Collider[] targets = Physics.OverlapSphere(pos, radius);

        if (targets.Length > 0)
        {
            foreach (Collider enemy in targets)
            {
                if (!enemy.CompareTag(attacker.tag))
                {
                    BaseEntity entity = enemy.GetComponent<BaseEntity>();

                    if (entity != null && !entity.flagDeath)
                    {
                        entity.TakeDamage(amount, thisEntity);
                    }
                }
            }
        }
    }

    public static GameObject FindEnemy(GameObject attacker, Vector3 pos, float radius)
    {
        Collider[] targets = Physics.OverlapSphere(pos, radius);

        if (targets.Length > 0)
        {
            foreach (Collider enemy in targets)
            {
                BaseEntity entity = enemy.GetComponent<BaseEntity>();
                if (entity != null && !entity.flagDeath && !enemy.CompareTag(attacker.tag))
                {
                    return enemy.gameObject;
                }
            }
        }

        return null;
    }
}
