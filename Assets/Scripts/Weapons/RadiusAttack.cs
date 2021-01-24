using System.Threading.Tasks;
using UnityEngine;

public class RadiusAttack : MonoBehaviour
{
    public static void RadiusDamage(GameObject attacker, Vector3 pos, float radius, WeaponDamageType damageType, float amount, LayerMask mask)
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(pos, radius, mask);

        if (targets.Length > 0)
        {
            BaseEntity thisEntity = attacker.GetComponent<BaseEntity>();
            foreach (Collider2D enemy in targets)
            {
                Tags entityTag = enemy.GetComponent<BaseTag>().entityTag;
                Tags attackerTag = attacker.GetComponent<BaseTag>().entityTag;
                if ((entityTag & attackerTag) == 0)
                {
                    BaseEntity entity = enemy.GetComponent<BaseEntity>();
                    if (entity != null && !entity.flagDeath)
                    {
                        entity.TakeDamage(amount, (int)damageType, thisEntity);
                    }
                }
            }
        }
    }

    public async static Task RadiusDamage(GameObject attacker, Vector3 pos, float radius, WeaponDamageType damageType, float amount, float delay, LayerMask mask)
    {
        await Task.Delay((int)(delay * 1000f));

        Collider2D[] targets = Physics2D.OverlapCircleAll(pos, radius, mask);

        if (targets.Length > 0)
        {
            BaseEntity thisEntity = attacker.GetComponent<BaseEntity>();
            foreach (Collider2D enemy in targets)
            {
                Tags entityTag = enemy.GetComponent<BaseTag>().entityTag;
                Tags attackerTag = attacker.GetComponent<BaseTag>().entityTag;
                if ((entityTag & attackerTag) == 0)
                {
                    BaseEntity entity = enemy.GetComponent<BaseEntity>();
                    if (entity != null && !entity.flagDeath)
                    {
                        entity.TakeDamage(amount, (int)damageType, thisEntity);
                    }
                }
            }
        }
    }
}
