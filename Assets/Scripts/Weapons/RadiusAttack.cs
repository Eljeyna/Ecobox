using System.Threading.Tasks;
using UnityEngine;

public class RadiusAttack : MonoBehaviour
{
    private static Collider2D[] targets = new Collider2D[10];
    private static Collider2D[] asyncTargets = new Collider2D[10];

    public static void RadiusDamage(GameObject attacker, Vector3 pos, float radius, WeaponDamageType damageType, float amount, LayerMask mask)
    {
        int length = Physics2D.OverlapCircleNonAlloc(pos, radius, targets, mask);

        if (length > 1)
        {            
            BaseEntity thisEntity = attacker.GetComponent<BaseEntity>();
            foreach (Collider2D enemy in targets)
            {
                if (enemy == null)
                {
                    break;
                }

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

    public async static Task RadiusDamage(GameObject attacker, Vector3 pos, float radius, WeaponDamageType damageType, float amount, int delay, LayerMask mask)
    {
        await Task.Delay(delay);

        int length = Physics2D.OverlapCircleNonAlloc(pos, radius, asyncTargets, mask);

        if (length > 1)
        {
            BaseEntity thisEntity = attacker.GetComponent<BaseEntity>();
            foreach (Collider2D enemy in asyncTargets)
            {
                if (enemy == null)
                {
                    break;
                }

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
