public class NoWeapon : Gun
{
    private void Update()
    {
        StatePerform();
    }

    public override void Attack()
    {
        Damage.RadiusDamage(gameObject, attackPoint.position, gunData.radius, gunData.damageType, gunData.damage, 1 << gameObject.layer);
    }

    public override bool Reload()
    {
        return true;
    }
}