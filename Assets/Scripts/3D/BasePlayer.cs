using System.Collections;
using UnityEngine;

public class BasePlayer : BaseEntity
{
    public HealthGUI healthText;
    public float invincibilityTime = 2.5f;

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
            return;
        }

        invinsibility = true;
        StartCoroutine(InvinsibilityTimer(amount));
    }

    public override void TakeHealth(float amount, BaseEntity healer)
    {
        health += amount;

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        healthText.ChangeText(health);
    }

    IEnumerator InvinsibilityTimer(float amount)
    {
        for (float i = health + amount; i >= health; i -= amount * Time.deltaTime)
        {
            healthText.ChangeText(i);
            yield return new WaitForEndOfFrame();
        }

        invinsibility = false;
        healthText.ChangeText(health);
    }

    public override void Die()
    {
        flagDeath = true;
    }
}
