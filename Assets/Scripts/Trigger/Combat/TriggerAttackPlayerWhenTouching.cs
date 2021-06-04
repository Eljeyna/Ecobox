using UnityEngine;

public class TriggerAttackPlayerWhenTouching : Trigger
{
    public float damage;
    public float force;
    public WeaponDamageType weaponDamageType;
    public AIEntity entity;
    public bool stun;

    public override void Use(Collider2D obj)
    {
        if (obj.TryGetComponent(out BaseEntity baseEntity))
        {
            baseEntity.TakeDamage(damage, (int)weaponDamageType, entity.thisEntity);
            
            if (force > 0f && obj.attachedRigidbody)
            {
                obj.attachedRigidbody.AddForce(transform.localScale * force, ForceMode2D.Impulse);
            }
        }

        if (stun)
        {
            entity.state = EntityState.Stun;
        }
    }
}
