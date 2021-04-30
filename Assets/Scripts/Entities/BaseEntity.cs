using UnityEngine;

public abstract class BaseEntity : MonoBehaviour
{
    public float maxHealth = 100f;
    public float health;
    [Range(0f, 1f)] public float healthPercent;
    [Range(-1f, 1f)] public float[] resistances;
    public bool invinsibility = false;
    public bool flagDeath;
    public BaseEntity attacker;

    public abstract void Awake();
    public abstract void TakeDamage(float amount, int attackType, BaseEntity attacker);
    public abstract void TakeDamagePercent(float amount, int attackType, BaseEntity attacker);
    public abstract void TakeHealth(float amount, BaseEntity healer);
    public abstract void TakeHealthPercent(float amount, BaseEntity healer);
    public abstract void SetMaxHealth(float amount);

    public abstract void Die();
}
