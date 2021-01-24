using System;

public class BasePlayer : BaseCommon
{
    public EventHandler OnHealthChanged;

    public override void TakeDamage(float amount, int attackType, BaseEntity attacker)
    {
        base.TakeDamage(amount, attackType, attacker);

        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    public override void TakeHealth(float amount, BaseEntity healer)
    {
        base.TakeHealth(amount, healer);

        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    public override void TakeDamagePercent(float amount, int attackType, BaseEntity attacker)
    {
        base.TakeDamagePercent(amount, attackType, attacker);

        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    public override void TakeHealthPercent(float amount, BaseEntity healer)
    {
        base.TakeHealthPercent(amount, healer);

        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }

    public override void SetMaxHealth(float amount)
    {
        base.SetMaxHealth(amount);

        OnHealthChanged?.Invoke(this, EventArgs.Empty);
    }
}
