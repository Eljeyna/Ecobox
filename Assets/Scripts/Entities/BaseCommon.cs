using System;

public class BaseCommon : BaseEntity
{
    public EventHandler OnHealthChanged;
    public EventHandler OnDie;
    
    public override void Awake()
    {
        health = maxHealth;
        healthPercent = health / maxHealth;
    }

    public override void TakeDamage(float amount, int attackType, BaseEntity attacker)
    {
        if (invinsibility || flagDeath)
        {
            return;
        }

        if (attackType == -1)
        {
            health -= amount;
        }
        else
        {
            health -= amount - (amount * resistances[attackType]);
        }

        healthPercent = health / maxHealth;
        this.attacker = attacker;

        if (health <= 0f)
        {
            health = 0f;
            healthPercent = 0f;
            Die();
        }
        
        EventHealthChanged();
    }

    public override void TakeHealth(float amount, BaseEntity healer)
    {
        if (flagDeath)
        {
            return;
        }
        
        health += amount;
        healthPercent = health / maxHealth;

        if (health > maxHealth)
        {
            health = maxHealth;
            healthPercent = 1f;
        }
        
        EventHealthChanged();
    }

    public override void TakeDamagePercent(float amount, int attackType, BaseEntity attacker)
    {
        if (invinsibility || flagDeath)
        {
            return;
        }

        if (attackType == -1)
        {
            healthPercent -= amount;
        }
        else
        {
            healthPercent -= amount - (amount * resistances[attackType]);
        }
        health = healthPercent * maxHealth;
        this.attacker = attacker;

        if (health <= 0f)
        {
            health = 0f;
            healthPercent = 0f;
            Die();
        }
        
        EventHealthChanged();
    }

    public override void TakeHealthPercent(float amount, BaseEntity healer)
    {
        if (flagDeath)
        {
            return;
        }
        
        healthPercent += amount;
        health = healthPercent * maxHealth;

        if (health > maxHealth)
        {
            health = maxHealth;
            healthPercent = 1f;
        }
        
        EventHealthChanged();
    }

    public override void SetMaxHealth(float amount)
    {
        maxHealth = amount;
        healthPercent = health / maxHealth;
        EventHealthChanged();
    }

    public override void Die()
    {
        flagDeath = true;
        OnDie?.Invoke(this, EventArgs.Empty);
    }

    public void EventHealthChanged()
    {
        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    private void OnValidate()
    {
        if (resistances == null || resistances.Length < 4)
        {
            resistances = new float[4];
        }
    }
}
