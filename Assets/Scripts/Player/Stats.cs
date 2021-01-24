using System;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public int maxStamina = 40;
    public int stamina;
    public int staminaRegen = 1;
    public float staminaTimeRegen = 0.2f;

    public int level = 1;
    public int exp;
    public int talentPoints;

    public float weight = 50f;

    [Space(10)]
    public int strength;
    public int agility;
    public int intelligence;

    [Space(10)]
    public int oratory;

    [Space(10)]
    public int money;

    public EventHandler OnStaminaChanged;

    private float nextStaminaRegen;

    [HideInInspector] public const int maxLevel = 60;
    [HideInInspector] public const int maxBonus = 20;

    /* Strength */
    [HideInInspector] public const float healthBonus = 2f;
    [HideInInspector] public const float weightBonus = 5f;

    /* Agility */
    [HideInInspector] public const float speedBonus = 0.1f;
    [HideInInspector] public const int staminaBonus = 20;
    [HideInInspector] public const int staminaRegenBonus = 1;

    /* Intelligence */
    [HideInInspector] public const int oratoryBonus = 1;

    /* All */
    [HideInInspector] public const float resistanceAll = 0.01f;

    private void Awake()
    {
        stamina = maxStamina;

        AddStrength(3);
        AddAgility(3);
        AddIntelligence(3);
    }

    private void Update()
    {
        if (stamina <= maxStamina && nextStaminaRegen <= Time.time)
        {
            stamina += staminaRegen;

            if (stamina > maxStamina)
            {
                stamina = maxStamina;
            }

            nextStaminaRegen = Time.time + staminaTimeRegen;
            OnStaminaChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    
    public void LevelUp()
    {
        if (level >= maxLevel)
        {
            return;
        }

        talentPoints++;
    }

    public void AddStrength(int amount)
    {
        if (strength >= maxBonus)
        {
            return;
        }

        strength += amount;
        AddStrengthBonus(amount);
    }

    public void AddAgility(int amount)
    {
        if (agility >= maxBonus)
        {
            return;
        }

        agility += amount;
        AddAgilityBonus(amount);
    }

    public void AddIntelligence(int amount)
    {
        if (intelligence >= maxBonus)
        {
            return;
        }

        intelligence += amount;
        AddIntelligenceBonus(amount);
    }

    public void AddStrengthBonus(int amount)
    {
        float heal = Player.Instance.thisPlayer.healthPercent;
        weight += weightBonus * amount;

        Player.Instance.thisPlayer.SetMaxHealth(Player.Instance.thisPlayer.maxHealth + (healthBonus * amount));
        Player.Instance.thisPlayer.TakeHealthPercent(heal, null);
        Player.Instance.thisPlayer.resistances[0] += resistanceAll * amount;
        Player.Instance.thisPlayer.resistances[1] += resistanceAll * amount;
    }

    public void AddAgilityBonus(int amount)
    {
        maxStamina += staminaBonus * amount;
        staminaRegen += staminaRegenBonus * amount;

        Player.Instance.speed += speedBonus * amount;
    }

    public void AddIntelligenceBonus(int amount)
    {
        oratory += oratoryBonus * amount;

        Player.Instance.thisPlayer.resistances[2] += resistanceAll * amount;
        Player.Instance.thisPlayer.resistances[3] += resistanceAll * amount;
    }
}
