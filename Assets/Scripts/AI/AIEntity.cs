using System;
using UnityEngine;
using Pathfinding;

public enum EntityState
{
    None    = 0,
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
    public EntityState state;
    public BaseTag thisTag;
    public BaseEntity thisEntity;
    public Transform target;
    public AIPath aiPath;
    public AIDestinationSetter aiEntity;
    public float defaultEndReachedDistance;
    public Gun weapon;

    public void InitializeEntity()
    {
        aiPath.maxSpeed = speed;
        defaultEndReachedDistance = aiPath.endReachedDistance;
    }

    public void StatePerform()
    {
        switch (state)
        {
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
                StateStun();
                break;
            case EntityState.Cast:
                StateStun();
                break;
            default:
                break;
        }
    }

    private void OnDestroy()
    {
        if (target != null)
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
