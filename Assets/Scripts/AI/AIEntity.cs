using System;
using UnityEngine;
using Pathfinding;

public enum EntityState
{
    None = 0,
    Normal,
    Dash,
    Stun,
    Attack,
    Cast,
}

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

    private void OnDestroy()
    {
        if (!ReferenceEquals(target, null))
        {
            Pool.Instance.AddToPool((int)PoolID.Target, target.gameObject);
        }
    }

    public abstract void StateNormal();
    public abstract void StateDash();
    public abstract void StateStun();
    public abstract void StateAttack();
    public abstract void StateCast();
    public abstract void Attack();
}
