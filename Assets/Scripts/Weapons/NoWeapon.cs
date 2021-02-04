using UnityEngine;

public class NoWeapon : Gun
{
    public AIEntity entity;
    public Transform attackPoint;

    private void Update()
    {
        if (entity.state == EntityState.Attack)
        {
            if (delay <= Time.time)
            {
                RadiusAttack.RadiusDamage(gameObject, attackPoint.position, gunData.radius, gunData.damageType, gunData.damage, 1 << gameObject.layer);
                entity.state = EntityState.Normal;
                this.enabled = false;
            }
        }
        else
        {
            delay = 0f;
            this.enabled = false;
        }
    }

    public override void PrimaryAttack()
    {
        nextAttack = Time.time + gunData.fireRatePrimary;

        if (gunData.delay == 0f)
        {
            RadiusAttack.RadiusDamage(gameObject, attackPoint.position, gunData.radius, gunData.damageType, gunData.damage, 1 << gameObject.layer);
        }
        else
        {
            entity.state = EntityState.Attack;
            delay = Time.time + gunData.delay;
        }
    }

    public override void SecondaryAttack()
    {
        return;
    }

    public override bool Reload()
    {
        return true;
    }
}