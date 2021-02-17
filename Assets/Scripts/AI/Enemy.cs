using System;

public class Enemy : AIEntity
{
    public InventoryDrop inventory;
    
    private void Awake()
    {
        InitializeEntity();
        InitializeTarget();
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
