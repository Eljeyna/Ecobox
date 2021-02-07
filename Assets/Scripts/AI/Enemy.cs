using UnityEngine;

public class Enemy : AIEntity
{
    private void Awake()
    {
        InitializeEntity();
    }

    private void Start()
    {
        state = EntityState.Normal;

        if (ReferenceEquals(aiEntity.target, null))
        {
            aiPath.endReachedDistance = defaultEndReachedDistance;
        }
        else if (aiEntity.target.TryGetComponent(out BaseTag theTag))
        {
            if ((theTag.entityTag & thisTag.entityTag) == 0)
            {
                aiPath.endReachedDistance = GetEndReachedDistance() - defaultEndReachedDistance;
            }
        }
    }

    private void Update()
    {
        StatePerform();
    }

    public override void StateNormal()
    {
        if (!aiPath.isActiveAndEnabled)
        {
            aiPath.enabled = true;
        }

        if (ReferenceEquals(aiEntity.target, null))
        {
            return;
        }

        if (!ReferenceEquals(weapon, null))
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

    public override void StateStun()
    {
        if (aiPath.isActiveAndEnabled)
        {
            aiPath.enabled = false;
        }
    }

    public override void StateDash()
    {
        return;
    }

    public override void StateAttack()
    {
        return;
    }

    public override void StateCast()
    {
        return;
    }

    public override void Attack()
    {
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

    public float GetEndReachedDistance()
    {
        if (aiEntity.target.TryGetComponent(out CapsuleCollider2D entityCollider))
        {
            return weapon.gunData.range + StaticGameVariables.GetReachedDistance(entityCollider);
        }
        else
        {
            return weapon.gunData.range;
        }
    }
}
