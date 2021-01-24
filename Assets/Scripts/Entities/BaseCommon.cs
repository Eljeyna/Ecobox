public class BaseCommon : BaseEntity
{
    public override void Awake()
    {
        health = maxHealth;
        healthPercent = health / maxHealth;
    }

    public override void TakeDamage(float amount, int attackType, BaseEntity attacker)
    {
        if (invinsibility)
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

        if (health <= 0)
        {
            health = 0;
            healthPercent = 0f;
            Die();
        }
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
    }

    public override void TakeDamagePercent(float amount, int attackType, BaseEntity attacker)
    {
        if (invinsibility)
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

        if (health <= 0)
        {
            health = 0;
            healthPercent = 0f;
            Die();
        }
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
    }

    public override void SetMaxHealth(float amount)
    {
        maxHealth = amount;
        healthPercent = health / maxHealth;
    }

    public override void Die()
    {
        flagDeath = true;
    }
}
