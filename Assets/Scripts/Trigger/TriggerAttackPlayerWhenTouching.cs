public class TriggerAttackPlayerWhenTouching : Trigger
{
    public float damage;
    public WeaponDamageType weaponDamageType;
    public AIEntity entity;
    public bool stun;

    public override void Use()
    {
        Player.Instance.thisEntity.TakeDamage(damage, (int)weaponDamageType, entity.thisEntity);

        if (stun)
        {
            entity.state = EntityState.Stun;
        }
    }
}
