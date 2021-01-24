using System.Threading.Tasks;
using UnityEngine;

public class NoWeapon : Gun
{
    public BaseEntity thisEntity;
    public Transform attackPoint;

    public override void PrimaryAttack()
    {
        nextAttack = Time.time + gunData.fireRatePrimary;

        if (gunData.delay == 0f)
        {
            RadiusAttack.RadiusDamage(gameObject, attackPoint.position, gunData.radius, gunData.damageType, gunData.damage, 1 << gameObject.layer);
        }
        else
        {
            _ = PrimaryAttackDelay();
        }
    }

    public async Task PrimaryAttackDelay()
    {
        await RadiusAttack.RadiusDamage(gameObject, attackPoint.position, gunData.radius, gunData.damageType, gunData.damage, gunData.delay, 1 << gameObject.layer);
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
