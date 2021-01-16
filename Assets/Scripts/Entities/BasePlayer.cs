using System;

public class BasePlayer : BaseEntity
{
    public EventHandler OnHealthChanged;

    public override void Awake()
    {
        health = maxHealth;
    }

    public override void TakeDamage(float amount, BaseEntity attacker)
    {
        if (invinsibility)
            return;

        health -= amount;
        this.attacker = attacker;

        if (health <= 0)
        {
            health = 0;
            Die();
        }

        //OnHealthChanged(this, EventArgs.Empty);
    }

    public override void TakeHealth(float amount, BaseEntity healer)
    {
        health += amount;

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        //OnHealthChanged(this, EventArgs.Empty);
    }

    public override void Die()
    {
        flagDeath = true;
    }

    public override float HealthPercent()
    {
        return health / maxHealth;
    }
}
