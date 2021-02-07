using UnityEngine;

public class BulletPistolSoldier : Bullet
{
    private void Update()
    {
        transform.position += transform.up * (bulletData.speed * Time.deltaTime);
        nextFade += Time.deltaTime;

        if (nextFade >= bulletData.timeFade)
        {
            nextFade = 0f;
            Pool.Instance.AddToPool(bulletData.index, gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            if (collision.collider.TryGetComponent(out BaseTag attackerTag))
            {
                if ((baseTag & attackerTag.entityTag) == 0)
                {
                    if (bulletData.radius > 0f)
                    {
                        RadiusAttack.RadiusDamage(owner.gameObject, transform.position, bulletData.radius,
                            bulletData.damageType, bulletData.damage, 1 << gameObject.layer);
                    }
                    else
                    {
                        BaseEntity baseEntity = collision.gameObject.GetComponent<BaseEntity>();
                        baseEntity.TakeDamage(bulletData.damage, 0, owner);
                    }
                }
            }
        }

        Pool.Instance.AddToPool(bulletData.index, gameObject);
    }
}
