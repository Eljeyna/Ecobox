using System;
using UnityEngine;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.layer)
        {
            case (int)GameLayers.Trigger:
                if (collision.TryGetComponent(out Trigger trigger))
                {
                    trigger.Use(gameObject.GetComponent<Collider2D>());
                }

                break;
        }
    }
}
