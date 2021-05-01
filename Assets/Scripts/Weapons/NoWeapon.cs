public class NoWeapon : Gun
{
    private void Update()
    {
        StatePerform();
    }

    public override void Attack()
    {
        Damage.RadiusDamage(gameObject, entity.transform.position + entity.targetDirection * gunData.range,
            gunData.radius, gunData.damageType, gunData.damage, gunData.impactForce, 1 << gameObject.layer);
    }

    public override bool Reload()
    {
        return true;
    }
}