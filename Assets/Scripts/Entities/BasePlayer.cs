using System;

public class BasePlayer : BaseEntity
{
    public EventHandler OnHealthChanged;

    public override void Awake()
    {
        health = maxHealth;
        healthPercent = health / maxHealth;
    }

    public override void TakeDamage(float amount, BaseEntity attacker)
    {
        if (invinsibility)
            return;

        health -= amount;
        healthPercent = health / maxHealth;
        this.attacker = attacker;

        if (health <= 0)
        {
            health = 0;
            healthPercent = 0f;
            Die();
        }

        //OnHealthChanged(this, EventArgs.Empty);
    }

    public override void TakeHealth(float amount, BaseEntity healer)
    {
        health += amount;
        healthPercent = health / maxHealth;

        if (health > maxHealth)
        {
            health = maxHealth;
            healthPercent = 1f;
        }

        //OnHealthChanged(this, EventArgs.Empty);
    }

    public override void TakeDamagePercent(float amount, BaseEntity attacker)
    {
        if (invinsibility)
            return;

        healthPercent -= amount;
        health = healthPercent * maxHealth;
        this.attacker = attacker;

        if (health <= 0)
        {
            health = 0;
            healthPercent = 0f;
            Die();
        }

        //OnHealthChanged(this, EventArgs.Empty);
    }

    public override void TakeHealthPercent(float amount, BaseEntity healer)
    {
        healthPercent += amount;
        health = healthPercent * maxHealth;

        if (health > maxHealth)
        {
            health = maxHealth;
            healthPercent = 1f;
        }

        //OnHealthChanged(this, EventArgs.Empty);
    }

    public override void Die()
    {
        flagDeath = true;
    }
}
