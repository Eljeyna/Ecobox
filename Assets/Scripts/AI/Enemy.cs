public class Enemy : AIEntity
{
    private void Awake()
    {
        InitializeEntity();
    }

    private void Start()
    {
        if (!aiEntity.target)
        {
            aiPath.endReachedDistance = defaultEndReachedDistance;
            return;
        }
        
        if (aiEntity.target.TryGetComponent(out BaseTag theTag))
        {
            if (Damage.IsEnemy(thisTag, theTag))
            {
                aiPath.endReachedDistance = GetEndReachedDistance() - defaultEndReachedDistance;
            }
        }
    }

    private void Update()
    {
        StatePerform();
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

    public override void StateSwing()
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
}
