using UnityEngine;

public enum WeaponDamageType
{
    Physical    = 0,   // Common Damage
    Chemical    = 1,   // Poison
    EMP         = 2,   // Electricity
    Thermal     = 3,   // Cold and Hot
}

public abstract class Gun : MonoBehaviour
{
    [HideInInspector] public float nextAttack;
    public bool reloading;
    public bool fireWhenEmpty;
    public float attackOffset;

    public float delay;
    
    public int clip;
    public int ammo;
    
    public GunData gunData;
    public AIEntity entity;

    public abstract void Attack();
    public abstract bool Reload();

    private void Awake()
    {
        if (!entity)
        {
            return;
        }
        
        Vector2 size = entity.thisCollider.size;
        attackOffset = (size.x + size.y) / 2;
    }

    public void StatePerform()
    {
        if (StaticGameVariables.isPause && delay != 0f)
        {
            delay = StaticGameVariables.WaitInPause(delay);
            return;
        }

        if (!entity)
        {
            return;
        }
    
        if (entity.state == EntityState.Swing)
        {
            if (delay <= Time.time)
            {
                Attack();

                if (gunData.lateDelay > 0f)
                {
                    delay = Time.time + gunData.lateDelay;
                    entity.state = EntityState.Attack;
                }
                else
                {
                    delay = 0f;
                    entity.state = EntityState.Normal;
                    
                    if (clip == -1)
                    {
                        this.enabled = false;
                    }
                }
            }
        }
        else if (entity.state == EntityState.Attack)
        {
            if (delay <= Time.time)
            {
                delay = 0f;
                entity.state = EntityState.Normal;
                
                if (clip == -1)
                {
                    this.enabled = false;
                }
            }
        }
        else
        {
            delay = 0f;
            
            if (clip == -1)
            {
                this.enabled = false;
            }
        }
    }

    public virtual void PrimaryAttack()
    {
        entity.StopForces();

        if (gunData.delay == 0f)
        {
            entity.state = EntityState.Attack;
            Attack();

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
        
        nextAttack = Time.time + gunData.fireRatePrimary;
    }
}
