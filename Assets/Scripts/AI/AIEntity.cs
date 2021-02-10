using System;
using UnityEngine;
using Pathfinding;
using UnityEngine.AddressableAssets;

public enum EntityState
{
    None = 0,
    Normal,
    Dash,
    Stun,
    Attack,
    Cast,
}

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BaseTag))]
[RequireComponent(typeof(BaseEntity))]
[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(Addressables_LoadSprite))]

[RequireComponent(typeof(BuffSystem))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(AIPath))]
[RequireComponent(typeof(AIDestinationSetter))]
[RequireComponent(typeof(Gun))]
public abstract class AIEntity : MonoBehaviour
{
    public float speed;
    public float speedSlow;
    public Rigidbody2D rb;
    public CapsuleCollider2D thisCollider;
    public EntityState state;
    public BaseTag thisTag;
    public BaseEntity thisEntity;
    public Transform target;
    public Vector3 targetPosition;
    public AIPath aiPath;
    public AIDestinationSetter aiEntity;
    public float defaultEndReachedDistance;
    public Gun weapon;
    public BuffSystem buffSystem;

    public void InitializeEntity()
    {
        aiPath.maxSpeed = speed;
        defaultEndReachedDistance = aiPath.endReachedDistance;
        target = null;
        if (aiEntity.target == null) // It's not a joke, just test this with ReferenceEquals(objA, objB)
        {
            aiEntity.target = null;
        }
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
            case EntityState.Attack:
                StateAttack();
                break;
            case EntityState.Cast:
                StateCast();
                break;
        }
    }
    
    public void Attack()
    {
        if (ReferenceEquals(weapon, null) || weapon.nextAttack > Time.time)
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
    
    public float GetEndReachedDistance()
    {
        if (aiEntity.target.TryGetComponent(out CapsuleCollider2D entityCollider))
        {
            return weapon.gunData.range + StaticGameVariables.GetReachedDistance(entityCollider) - defaultEndReachedDistance * 2;
        }
        else
        {
            return weapon.gunData.range;
        }
    }

    public void OnPause(object sender, EventArgs e)
    {
        aiPath.enabled = !StaticGameVariables.isPause;
    }
    
    private void OnEnable()
    {
        StaticGameVariables.OnPauseGame += OnPause;
    }
    
    private void OnDisable()
    {
        StaticGameVariables.OnPauseGame -= OnPause;
    }
    
    private void OnDestroy()
    {
        aiEntity.target = null;
        if (!ReferenceEquals(target, null))
        {
            Addressables.ReleaseInstance(target.gameObject);
        }
        
        Addressables.ReleaseInstance(gameObject);
    }

    public abstract void StateNormal();
    public abstract void StateDash();
    public abstract void StateStun();
    public abstract void StateAttack();
    public abstract void StateCast();
}
