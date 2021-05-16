using System;

public class AIEnemy : AIEntity
{
    public InventoryDrop inventory;
    
    private void Start()
    {
        InitializeEntity();
        //InitializeTarget();
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
