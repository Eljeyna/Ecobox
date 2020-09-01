using System.Collections;
using UnityEngine;

public class BasePlayer : BaseEntity
{
    public HealthGUI healthText;
    public float invincibilityTime = 2f;

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
        //healthText.ChangeText(health);
    }

    IEnumerator InvinsibilityTimer(float amount)
    {
        //yield return new WaitForSeconds(invincibilityTime);

        for (float i = health + amount; i >= health; i -= amount * Time.deltaTime)
        {
            healthText.ChangeText(i);
            yield return new WaitForSeconds(Time.deltaTime);
        }

        invinsibility = false;
        healthText.ChangeText(health);
    }

    public override void Die()
    {
        health = maxHealth;
        healthText.ChangeText(maxHealth);
    }
}
