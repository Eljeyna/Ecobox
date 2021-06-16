using UnityEngine;

public class TriggerAttackPlayerWhenTouching : Trigger
{
    public AIEntity entity;
    public float damage;
    public float force;
    public bool stun;
    public float stunTime;
    public WeaponDamageType weaponDamageType;

    public override void Use(Collider2D obj)
    {
        if (obj.TryGetComponent(out BaseEntity baseEntity) && obj.TryGetComponent(out BaseTag entityTag) && Damage.IsEnemy(entity.thisTag, entityTag))
        {
            baseEntity.TakeDamage(damage, (int)weaponDamageType, entity.thisEntity);
            entity.rb.AddForce(-entity.transform.localScale * force, ForceMode2D.Impulse);

            if (force > 0f && obj.attachedRigidbody)
            {
                obj.attachedRigidbody.AddForce(entity.transform.localScale * force, ForceMode2D.Impulse);
            }
        }

        if (stun)
        {
            entity.Standing();
            entity.stunTime = Time.time + stunTime;
            entity.state = EntityState.Stun;
        }
    }
}
