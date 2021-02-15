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

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BaseTag))]
[RequireComponent(typeof(BaseEntity))]
[RequireComponent(typeof(Seeker))]

[RequireComponent(typeof(BuffSystem))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(AIPath))]
[RequireComponent(typeof(AIDestinationSetter))]
[RequireComponent(typeof(Gun))]
[RequireComponent(typeof(Dash))]
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
    public Vector3 targetPosition;
    public BuffSystem buffSystem;
    public Animator animations;
    public AIPath aiPath;
    public AIDestinationSetter aiEntity;
    public Gun weapon;
    public Dash dash;
    
    public float deathTime;

    [HideInInspector] public Collider2D[] entity = new Collider2D[1];
    [HideInInspector] public bool isEnemy;
    [HideInInspector] public Vector3 dashDirection;

    public void InitializeEntity()
    {
        state = EntityState.Normal;
        aiEntity.target = null;
        Speed = speed;
        defaultEndReachedDistance = aiPath.endReachedDistance;
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

    public void StateNormal()
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
            if (ReferenceEquals(entity, null))
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
    
    public void StateDash()
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
    
    public void StateStun()
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
    
    public void StateDeath()
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

        if (gameObject != Player.Instance.gameObject)
        {
            Addressables.ReleaseInstance(gameObject);
            return;
        }

        Player.Instance.OnLoad();
    }

    public void SetAnimation()
    {
        animations.SetInteger(StaticGameVariables.animationKeyID, (int)state);
        animations.SetBool(StaticGameVariables.animationMoveKeyID, !aiPath.reachedDestination);
    }
    
    public void Attack()
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
            aiPath.endReachedDistance = defaultEndReachedDistance;
            isEnemy = false;
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

    public void OnPause(object sender, EventArgs e)
    {
        aiPath.enabled = !StaticGameVariables.isPause;
        animations.speed = StaticGameVariables.isPause ? 0f : 1f;
    }

    public virtual void OnDie(object sender, EventArgs e)
    {
        state = EntityState.Death;
        deathTime = Time.time + 3f;
    }

    public virtual void EventEnable()
    {
        StaticGameVariables.OnPauseGame += OnPause;
        thisEntity.OnDie += OnDie;
    }
    
    public virtual void EventDisable()
    {
        StaticGameVariables.OnPauseGame -= OnPause;
        thisEntity.OnDie -= OnDie;
    }
    
    public virtual void EventDestroy()
    {
        StaticGameVariables.OnPauseGame -= OnPause;
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
