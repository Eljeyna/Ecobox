using UnityEngine;

public class Damage : MonoBehaviour
{
    private static Collider2D[] targets = new Collider2D[10];

    public static void RadiusDamage(GameObject attacker, Vector3 pos, float radius, WeaponDamageType damageType, float amount, float force, LayerMask mask)
    {
        int length = Physics2D.OverlapCircleNonAlloc(pos, radius, targets, mask);

        if (length > 1)
        {            
            foreach (Collider2D enemy in targets)
            {
                if (enemy && enemy.TryGetComponent(out BaseTag enemyTag) && attacker.TryGetComponent(out BaseTag attackerTag))
                {
                    if (IsEnemy(enemyTag, attackerTag))
                    {
                        if (enemy.TryGetComponent(out BaseEntity entity) && attacker.TryGetComponent(out BaseEntity thisEntity))
                        {
                            entity.TakeDamage(amount, (int)damageType, thisEntity);

                            if (force > 0f && enemy.attachedRigidbody)
                            {
                                enemy.attachedRigidbody.AddForce(-enemy.transform.localScale * force, ForceMode2D.Impulse);
                            }
                        }
                    }
                }
            }
        }
    }

    public static bool IsEnemy(BaseTag tagA, BaseTag tagB)
    {
        return (tagA.entityTag & tagB.entityTag) == 0;
    }
    
    public static bool IsAlly(BaseTag tagA, BaseTag tagB)
    {
        return (tagA.entityTag & tagB.entityTag) != 0;
    }
}
