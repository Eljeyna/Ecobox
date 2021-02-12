using UnityEngine;

public class NoWeapon : Gun
{
    public AIEntity entity;
    public Transform attackPoint;

    private void Update()
    {
        if (StaticGameVariables.isPause && delay != 0f)
        {
            delay = StaticGameVariables.WaitInPause(delay);
            return;
        }
    
        if (entity.state == EntityState.Swing)
        {
            if (delay <= Time.time)
            {
                Damage.RadiusDamage(gameObject, attackPoint.position, gunData.radius, gunData.damageType, gunData.damage, 1 << gameObject.layer);

                if (gunData.lateDelay > 0f)
                {
                    delay = Time.time + gunData.lateDelay;
                    entity.state = EntityState.Attack;
                }
                else
                {
                    delay = 0f;
                    entity.state = EntityState.Normal;
                    this.enabled = false;
                }
            }
        }
        else if (entity.state == EntityState.Attack)
        {
            if (delay <= Time.time)
            {
                delay = 0f;
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
            entity.state = EntityState.Attack;
            Damage.RadiusDamage(gameObject, attackPoint.position, gunData.radius, gunData.damageType, gunData.damage, 1 << gameObject.layer);

            if (gunData.lateDelay > 0f)
            {
                delay = Time.time + gunData.lateDelay;
            }
        }
        else
        {
            entity.state = EntityState.Swing;
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