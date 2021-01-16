public class BaseEnemy : BaseEntity
{
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
        }
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

    public override void Die()
    {
        flagDeath = true;
    }
}
