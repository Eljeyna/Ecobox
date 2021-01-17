using UnityEngine;

public class RadiusAttack : MonoBehaviour
{
    public static void RadiusDamage(GameObject attacker, Vector3 pos, float radius, float amount, LayerMask mask)
    {
        Collider[] targets = Physics.OverlapSphere(pos, radius, mask);

        if (targets.Length > 0)
        {
            BaseEntity thisEntity = attacker.GetComponent<BaseEntity>();
            foreach (Collider enemy in targets)
            {
                Tags entityTag = enemy.GetComponent<BaseTag>().entityTag;
                Tags attackerTag = attacker.GetComponent<BaseTag>().entityTag;
                if ((entityTag & attackerTag) == 0)
                {
                    BaseEntity entity = enemy.GetComponent<BaseEntity>();
                    if (entity != null && !entity.flagDeath)
                    {
                        entity.TakeDamage(amount, 0, thisEntity);
                    }
                }
            }
        }
    }
}
