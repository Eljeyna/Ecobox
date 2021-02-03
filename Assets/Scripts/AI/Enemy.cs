using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    public BaseTag thisTag;
    public AIPath aiPath;
    public AIDestinationSetter aiEntity;
    public Rigidbody2D rb;
    public Gun weapon;

    public EntityState state;
    
    private float defaultEndReachedDistance;

    private void Awake()
    {
        defaultEndReachedDistance = aiPath.endReachedDistance;
    }

    private void Start()
    {
        state = EntityState.Normal;

        if (aiEntity.target != null)
        {
            BaseTag tag = aiEntity.target.GetComponent<BaseTag>();
            if (tag != null && (tag.entityTag & thisTag.entityTag) == 0)
            {
                aiPath.endReachedDistance = GetEndReachedDistance();
            }
        }
        else
        {
            aiPath.endReachedDistance = defaultEndReachedDistance;
        }
    }

    private void Update()
    {
        switch (state)
        {
            case EntityState.Normal:
                StateNormal();
                break;
            case EntityState.Stun:
                StateStun();
                break;
            default:
                break;
        }
    }

    private void StateNormal()
    {
        if (!aiEntity.isActiveAndEnabled)
        {
            aiEntity.enabled = true;
        }

        if (aiEntity.target == null)
        {
            return;
        }

        if (weapon != null)
        {
            float distance = Vector2.Distance(rb.position, aiEntity.target.position);

            if (distance <= aiPath.endReachedDistance)
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

    private void StateStun()
    {
        if (aiEntity.isActiveAndEnabled)
        {
            aiEntity.enabled = false;
        }

        return;
    }

    public virtual void Attack()
    {
        if (weapon.nextAttack <= Time.time)
        {
            if (weapon.clip == 0)
            {
                weapon.fireWhenEmpty = true;
            }

            weapon.PrimaryAttack();
        }
    }

    public float GetEndReachedDistance()
    {
        CapsuleCollider2D collider = aiEntity.target.GetComponent<CapsuleCollider2D>();
        if (collider != null)
        {
            return weapon.gunData.range + StaticGameVariables.GetReachedDistance(collider);
        }
        else
        {
            return weapon.gunData.range;
        }
    }
}
