using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

public enum EntityState
{
    None   = 0,
    Normal = 1,     // Idle/Move/Jump animation
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
        }
    }
    public bool isGrounded;
    public bool isJumping;
    
    public float speed;
    public Rigidbody2D rb;
    public CapsuleCollider2D thisCollider;
    public EntityState state;
    public BaseTag thisTag;
    public BaseCommon thisEntity;
    public Transform target;
    public Vector3 targetDirection = new Vector3(1f, 0f, 0f);
    public BuffSystem buffSystem;
    public Animator animations;
    public Gun weapon;

    public float deathTime;

    protected RaycastHit2D[] groundCheck = new RaycastHit2D[2];
    protected Collider2D[] entity = new Collider2D[2];
    protected bool isEnemy;
    protected Vector3 dashDirection;

    [HideInInspector] public float moveVelocity;

    private void FixedUpdate()
    {
        if (StaticGameVariables.isPause)
        {
            return;
        }

        isGrounded = IsGrounded();
    }

    public void InitializeEntity()
    {
        state = EntityState.Normal;

        if (Player.Instance)
        {
            UpdateTarget(Player.Instance.transform);
        }

        Speed = speed;
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
        if (!target)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
            return;
        }

        float distance = Vector2.Distance(rb.position, target.position);
        
        if (distance <= 2f)
        {
            if (entity[0])
            {
                target = null;
                return;
            }

            if (isEnemy)
            {
                Attack();
            }
        }

        float angle = StaticGameVariables.GetAngleBetweenPositions(target.position, transform.position);

        if (angle <= 90f && angle >= -90f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        moveVelocity = transform.localScale.x;

        rb.velocity = new Vector2(moveVelocity * Speed, rb.velocity.y);
    }

    public virtual void StateDash()
    {
        return;
    }
    
    public virtual void StateStun()
    {
        rb.velocity = new Vector2(0f, rb.velocity.y);
    }

    public virtual void StateSwing()
    {
        return;
    }

    public virtual void StateAttack()
    {
        return;
    }

    public virtual void StateCast()
    {
        return;
    }

    public virtual void StateDeath()
    {

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
        animations.SetBool(StaticGameVariables.animationMoveKeyID, moveVelocity != 0f);
        animations.SetBool(StaticGameVariables.animationJumpKeyID, !isGrounded);
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
        targetDirection = (target.position - transform.position).normalized;
        weapon.PrimaryAttack();
    }

    public bool IsGrounded()
    {
        int hits = Physics2D.BoxCastNonAlloc(thisCollider.bounds.center, thisCollider.bounds.size, 0f, Vector2.down, groundCheck, 0.1f);
        return hits > 1;
    }

    public void UpdateTarget(Transform newTarget)
    {
        target = newTarget;

        if (target.TryGetComponent(out BaseTag theTag) && Damage.IsEnemy(thisTag, theTag))
        {
            isEnemy = true;
            //aiPath.endReachedDistance = GetEndReachedDistance() - defaultEndReachedDistance;
        }
        else
        {
            isEnemy = false;
            //aiPath.endReachedDistance = defaultEndReachedDistance;
        }
    }
    
    public float GetEndReachedDistance()
    {
        /*if (aiEntity.target.TryGetComponent(out CapsuleCollider2D entityCollider))
        {
            return weapon.gunData.range + StaticGameVariables.GetReachedDistance(entityCollider) - defaultEndReachedDistance * 2;
        }*/
        
        return weapon.gunData.range;
    }

    public virtual void OnPause(object sender, EventArgs e)
    {
        rb.simulated = !StaticGameVariables.isPause;
        moveVelocity = 0f;
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
        rb.simulated = false;
        rb.isKinematic = true;
        deathTime = Time.time + 3f;
        state = EntityState.Death;
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
