using UnityEngine;

public class BulletPistol : Bullet
{
    private void Update()
    {
        if (StaticGameVariables.isPause)
        {
            return;
        }
        
        transform.position += transform.right * (bulletData.speed * Time.deltaTime);
        nextFade += Time.deltaTime;

        if (nextFade >= bulletData.timeFade)
        {
            nextFade = 0f;
            Pool.Instance.AddToPool(bulletData.index, gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8) // Entities
        {
            if (collision.collider.gameObject == owner.gameObject)
            {
                return;
            }
            
            if (collision.collider.TryGetComponent(out BaseTag attackerTag))
            {
                if (Damage.IsEnemy(baseTag, attackerTag))
                {
                    if (bulletData.radius > 0f)
                    {
                        Damage.RadiusDamage(owner.gameObject, transform.position, bulletData.radius,
                            bulletData.damageType, bulletData.damage, 1 << gameObject.layer);
                    }
                    else
                    {
                        if (collision.collider.TryGetComponent(out BaseEntity baseEntity))
                        {
                            baseEntity.TakeDamage(bulletData.damage, 0, owner);
                        }
                    }
                }
            }
        }

        Pool.Instance.AddToPool(bulletData.index, gameObject);
    }
}
