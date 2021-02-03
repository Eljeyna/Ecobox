using UnityEngine;

public class NoWeapon : Gun
{
    public BaseEntity thisEntity;
    public Transform attackPoint;

    private void Update()
    {
        if (delay != 0f && delay <= Time.time)
        {
            delay = 0f;
            RadiusAttack.RadiusDamage(gameObject, attackPoint.position, gunData.radius, gunData.damageType, gunData.damage, 1 << gameObject.layer);
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