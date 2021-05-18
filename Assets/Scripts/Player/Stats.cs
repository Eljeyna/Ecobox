using System;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public int maxStamina = 100;
    public int stamina;
    public int staminaRegen = 2;
    public float staminaTimeRegen = 0.2f;
    public float staminaTimeRegenWhenUse = 1.5f;

    [Space(10)]
    public int money;
    public int qualitativeMaterial;
    public int badQualityMaterial;

    public EventHandler OnStaminaChanged;

    public float nextStaminaRegen;

    public void Initialize()
    {
        if (Settings.Instance.gameIsLoaded)
        {
            return;
        }

        stamina = maxStamina;
    }
    
    public void StaminaAction(int amount)
    {
        stamina -= amount;
        nextStaminaRegen = Time.time + staminaTimeRegenWhenUse;
        OnStaminaChanged?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        if (StaticGameVariables.isPause)
        {
            return;
        }
        
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
}
