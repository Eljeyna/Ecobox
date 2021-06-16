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
    Trigger = 26,
    UpgradeZone = 27,
    TrashBin = 28,
    Minimap = 29,
    Items = 30,
    Obstacles = 31,
}

public enum GameLayerMasks
{
    Obstacles = (1 << 31)
}

public abstract class AIEntity : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    public Collider2D thisCollider;
    public EntityState state;
    public BaseTag thisTag;
    public BaseCommon thisEntity;
    public Transform target;
    public Vector3 targetDirection = new Vector3(1f, 0f, 0f);
    public BuffSystem buffSystem;
    public Animator animations;
    public Gun weapon;

    public float deathTime;
    public float stunTime;

    protected RaycastHit2D[] groundCheck = new RaycastHit2D[2];
    protected Collider2D[] entity = new Collider2D[1];
    protected bool isEnemy;
    protected float needDistanceBetweenTarget;

    [HideInInspector] public float moveVelocity;

    [Space(10)]
    public bool isGrounded;
    public bool isJumping;

    public void InitializeEntity()
    {
        state = EntityState.Normal;

        if (Player.Instance)
        {
            UpdateTarget(Player.Instance.transform);
        }
    }

    public async void InitializeTarget()
    {
        GameObject newTarget = await Pool.Instance.GetFromPoolAsync((int)PoolID.Target);
        target = newTarget.transform;
    }

    public void StatePerform()
    {
        if (Game.isPause)
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
            Standing();
            return;
        }

        float distance = Vector2.Distance(rb.position, target.position);
        
        if (distance <= needDistanceBetweenTarget)
        {
            if (entity[0])
            {
                target = null;
                return;
            }

            if (isEnemy)
            {
                Attack();
                return;
            }
        }

        float angle = Game.GetAngleBetweenPositions(target.position, transform.position);

        if (angle <= 90f && angle >= -90f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        moveVelocity = transform.localScale.x * speed - rb.velocity.x;
        rb.velocity += new Vector2(moveVelocity, 0f);
    }

    public virtual void StateDash()
    {
        return;
    }
    
    public virtual void StateStun()
    {
        if (stunTime > Time.time)
        {
            return;
        }

        state = EntityState.Normal;

        if ((thisTag.entityTag & Tags.FL_PLAYER) != 0)
        {
            thisEntity.invinsibility = false;
        }
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
            if (deathTime - 1f <= Time.time)
            {
                spriteRenderer.color = new Color(deathTime - Time.time, spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b);
            }
        
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
        animations.SetInteger(Game.animationKeyID, (int)state);
        animations.SetBool(Game.animationMoveKeyID, rb.velocity.x != 0f);
        animations.SetBool(Game.animationJumpKeyID, !isGrounded);
    }

    public virtual void Attack()
    {
        if (!weapon)
        {
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

    public void Standing()
    {
        if (Mathf.Abs(rb.velocity.x) <= speed)
        {
            moveVelocity = 0f;
            rb.velocity = new Vector2(moveVelocity, rb.velocity.y);
        }
    }

    public bool IsGrounded()
    {
        int hits = Physics2D.BoxCastNonAlloc(thisCollider.bounds.center, thisCollider.bounds.size, 0f, Vector2.down, groundCheck, 0.1f, (int)GameLayerMasks.Obstacles);
        return hits > 0;
    }

    public void UpdateTarget(Transform newTarget)
    {
        target = newTarget;

        if (target.TryGetComponent(out BaseTag theTag) && Damage.IsEnemy(thisTag, theTag))
        {
            isEnemy = true;
            needDistanceBetweenTarget = GetEndReachedDistance();
        }
        else
        {
            isEnemy = false;
            needDistanceBetweenTarget = 0.1f;
        }
    }
    
    public float GetEndReachedDistance()
    {
        if (!weapon)
        {
            if (target.TryGetComponent(out Collider2D entityCollider))
            {
                return Game.GetReachedDistance(entityCollider);
            }

            return thisCollider.bounds.size.x + thisCollider.bounds.size.y;
        }
        
        if (target.TryGetComponent(out Collider2D entityCollider1))
        {
            return Game.GetReachedDistance(entityCollider1) - weapon.gunData.range;
        }
        
        return weapon.gunData.range;
    }

    public virtual void OnDamaged(object sender, HealthArguments healthArguments)
    {
        if (thisEntity.flagDeath || healthArguments.isDamageOrHeal)
        {
            return;
        }

        Standing();
        state = EntityState.Stun;
        stunTime = Time.time + 0.5f;

        if ((thisTag.entityTag & Tags.FL_PLAYER) != 0)
        {
            thisEntity.invinsibility = true;
        }
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
        thisEntity.OnHealthChanged += OnDamaged;
        thisEntity.OnDie += OnDie;
    }
    
    public virtual void EventDisable()
    {
        thisEntity.OnHealthChanged -= OnDamaged;
        thisEntity.OnDie -= OnDie;
    }
    
    public virtual void EventDestroy()
    {
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
