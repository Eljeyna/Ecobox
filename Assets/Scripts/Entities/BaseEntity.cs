using UnityEngine;

public abstract class BaseEntity : MonoBehaviour
{
    public float maxHealth = 100f;
    public float health;
    public float healthPercent;
    public float[] resistance;
    public bool invinsibility = false;
    public bool flagDeath;
    public BaseEntity attacker;

    public abstract void Awake();
    public abstract void TakeDamage(float amount, int attackType, BaseEntity attacker);
    public abstract void TakeDamagePercent(float amount, int attackType, BaseEntity attacker);
    public abstract void TakeHealth(float amount, BaseEntity healer);
    public abstract void TakeHealthPercent(float amount, BaseEntity healer);
    public abstract void Die();
}
