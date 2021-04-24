using System;
using UnityEngine;
using Pathfinding;
using UnityEngine.AddressableAssets;

public enum EntityState
{
    None   = 0,
    Normal = 1,     // Idle/Move animation
    Dash   = 2,
    Stun   = 3,
    Swing  = 4,
    Attack = 5,
    Cast   = 6,
    Death  = 7
}

public enum GameLayers
{
    Entities = 8,
    Spawners = 20,
    Minimap = 29,
    Items = 30,
    Obstacles = 31,
}

public abstract class AIEntity : MonoBehaviour
{
    public float Speed
    {
        get => speed;
        set
        {
            speed = value;
            aiPath.maxSpeed = speed;
        }
    }
    
    public float speed;
    public float defaultEndReachedDistance;
    public Rigidbody2D rb;
    public CapsuleCollider2D thisCollider;
    public EntityState state;
    public BaseTag thisTag;
    public BaseCommon thisEntity;
    public Transform target;
    public Vector3 targetDirection;
    public BuffSystem buffSystem;
    public Animator animations;
    public AIPath aiPath;
    public AIDestinationSetter aiEntity;
    public Gun weapon;
    public Dash dash;
    
    public float deathTime;

    [HideInInspector] public Collider2D[] entity = new Collider2D[2];
    [HideInInspector] public bool isEnemy;
    [HideInInspector] public Vector3 dashDirection;

    public void InitializeEntity()
    {
        state = EntityState.Normal;
        //aiEntity.target = null;
        if (Player.Instance)
        {
            UpdateTarget(Player.Instance.transform);
        }
        Speed = speed;
        defaultEndReachedDistance = aiPath.endReachedDistance;
    }

    public async void InitializeTarget()
    {
        GameObject newTarget = await Pool.Instance.GetFromPoolAsync((int)PoolID.Target);
        target = newTarget.transform;
    }

    public void StatePerform()
    {
        if (StaticGameVariables.isPause)
        {
            return;
        }

        switch (state)
        {
            case EntityState.None:
                break;
            case EntityState.Normal:
                StateNormal();
                break;
            case EntityState.Dash:
                StateDash();
                break;
            case EntityState.Stun:
                StateStun();
                break;
            case EntityState.Swing:
                StateSwing();
                break;
            case EntityState.Attack:
                StateAttack();
                break;
            case EntityState.Cast:
                StateCast();
                break;
            case EntityState.Death:
                StateDeath();
                break;
            default:
                state = EntityState.None;
                break;
        }
        
        SetAnimation();
    }

    public virtual void StateNormal()
    {
        if (!aiPath.isActiveAndEnabled)
        {
            aiPath.enabled = true;
        }

        if (!aiEntity.target)
        {
            return;
        }

        float distance = Vector2.Distance(rb.position, aiEntity.target.position);
        
        if (distance <= aiPath.endReachedDistance)
        {
            if (entity[0])
            {
                aiEntity.target = null;
                return;
            }

            if (isEnemy)
            {
                Attack();
            }
        }

        float angle = StaticGameVariables.GetAngleBetweenPositions(aiEntity.target.position, transform.position);

        if (angle <= 90f && angle >= -90f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
    
    public virtual void StateDash()
    {
        if (dash.nextDash <= Time.time)
        {
            aiEntity.enabled = true;
            aiPath.enabled = true;
            rb.velocity = Vector2.zero;
            state = EntityState.Normal;
        }
        else
        {
            float dashSpeed = dash.dashSpeed.Evaluate(dash.dashEvaluateTime);
            rb.velocity = dashDirection * dashSpeed;
            dash.dashEvaluateTime += Time.deltaTime;
        }
    }
    
    public virtual void StateStun()
    {
        if (aiPath.isActiveAndEnabled)
        {
            aiPath.enabled = false;
        }
    }

    public void StateSwing()
    {
        return;
    }

    public void StateAttack()
    {
        return;
    }

    public void StateCast()
    {
        return;
    }
    
    public virtual void StateDeath()
    {
        aiPath.enabled = false;

        if (deathTime > Time.time)
        {
            return;
        }
        
        if (!gameObject)
        {
            return;
        }
        
        Addressables.ReleaseInstance(gameObject);
    }

    public virtual void SetAnimation()
    {
        animations.SetInteger(StaticGameVariables.animationKeyID, (int)state);
        animations.SetBool(StaticGameVariables.animationMoveKeyID, !aiPath.reachedDestination);
    }
    
    public virtual void Attack()
    {
        if (!weapon)
        {
            state = EntityState.Normal;
            return;
        }
        
        if (weapon.nextAttack > Time.time)
        {
            return;
        }

        if (weapon.clip == 0)
        {
            weapon.fireWhenEmpty = true;
        }

        weapon.enabled = true;
        targetDirection = (aiEntity.target.position - transform.position).normalized;
        weapon.PrimaryAttack();
    }

    public void UpdateTarget(Transform newTarget)
    {
        aiEntity.target = newTarget;
        
        if (aiEntity.target.TryGetComponent(out BaseTag theTag) && Damage.IsEnemy(thisTag, theTag))
        {
            isEnemy = true;
            aiPath.endReachedDistance = GetEndReachedDistance() - defaultEndReachedDistance;
        }
        else
        {
            isEnemy = false;
            aiPath.endReachedDistance = defaultEndReachedDistance;
        }
    }
    
    public float GetEndReachedDistance()
    {
        if (aiEntity.target.TryGetComponent(out CapsuleCollider2D entityCollider))
        {
            return weapon.gunData.range + StaticGameVariables.GetReachedDistance(entityCollider) - defaultEndReachedDistance * 2;
        }
        
        return weapon.gunData.range;
    }

    public virtual void OnPause(object sender, EventArgs e)
    {
        rb.simulated = !StaticGameVariables.isPause;
        aiPath.enabled = !StaticGameVariables.isPause;
        animations.speed = StaticGameVariables.isPause ? 0f : 1f;
    }

    public void OnDamaged(object sender, EventArgs e)
    {
        /*if (!target && transform.parent && transform.parent.TryGetComponent(out EntityMaker entityMaker))
        {
            entityMaker.isTrigger = true;
            entityMaker.target = thisEntity.attacker.transform;
            entityMaker.UpdateTarget();
        }*/
    }

    public virtual void OnDie(object sender, EventArgs e)
    {
        Destroy(thisCollider);
        state = EntityState.Death;
        deathTime = Time.time + 3f;
    }

    public virtual void EventEnable()
    {
        StaticGameVariables.OnPauseGame += OnPause;
        thisEntity.OnHealthChanged += OnDamaged;
        thisEntity.OnDie += OnDie;
    }
    
    public virtual void EventDisable()
    {
        StaticGameVariables.OnPauseGame -= OnPause;
        thisEntity.OnHealthChanged -= OnDamaged;
        thisEntity.OnDie -= OnDie;
    }
    
    public virtual void EventDestroy()
    {
        StaticGameVariables.OnPauseGame -= OnPause;
        thisEntity.OnHealthChanged -= OnDamaged;
        thisEntity.OnDie -= OnDie;
        
        if (target)
        {
            Addressables.ReleaseInstance(target.gameObject);
        }
        
        Addressables.ReleaseInstance(gameObject);
    }

    private void OnEnable()
    {
        EventEnable();
    }

    private void OnDisable()
    {
        EventDisable();
    }

    private void OnDestroy()
    {
        EventDestroy();
    }
}
