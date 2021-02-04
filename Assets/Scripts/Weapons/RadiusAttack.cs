using System.Threading.Tasks;
using UnityEngine;

public class RadiusAttack : MonoBehaviour
{
    private static Collider2D[] targets = new Collider2D[10];

    public static void RadiusDamage(GameObject attacker, Vector3 pos, float radius, WeaponDamageType damageType, float amount, LayerMask mask)
    {
        int length = Physics2D.OverlapCircleNonAlloc(pos, radius, targets, mask);

        if (length > 1)
        {            
            foreach (Collider2D enemy in targets)
            {
                if (enemy == null)
                {
                    break;
                }

                if (enemy.TryGetComponent(out BaseTag enemyTag) && attacker.TryGetComponent(out BaseTag attackerTag))
                {
                    if ((enemyTag.entityTag & attackerTag.entityTag) == 0)
                    {
                        if (enemy.TryGetComponent(out BaseEntity entity) && attacker.TryGetComponent(out BaseEntity thisEntity))
                        {
                            entity.TakeDamage(amount, (int) damageType, thisEntity);
                        }
                    }
                }
            }
        }
    }
}
