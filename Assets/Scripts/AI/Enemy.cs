using System;

public class Enemy : AIEntity
{
    public InventoryDrop inventory;
    
    private void Awake()
    {
        InitializeEntity();
    }

    private void Start()
    {
        if (transform.parent)
        {
            if (transform.parent.TryGetComponent(out EntityMaker entityMaker))
            {
                aiEntity.target = entityMaker.target;
            }
        }
        
        if (!aiEntity.target)
        {
            aiPath.endReachedDistance = defaultEndReachedDistance;
            return;
        }
        
        if (aiEntity.target.TryGetComponent(out BaseTag theTag) && Damage.IsEnemy(thisTag, theTag))
        {
            isEnemy = true;
            aiPath.endReachedDistance = GetEndReachedDistance() - defaultEndReachedDistance;
        }
        else
        {
            isEnemy = false;
        }
    }

    private void Update()
    {
        StatePerform();
    }

    public override void OnDie(object sender, EventArgs e)
    {
        if (GameDirector.Instance)
        {
            inventory.Drop();
        }
        
        base.OnDie(sender, e);
    }
}
