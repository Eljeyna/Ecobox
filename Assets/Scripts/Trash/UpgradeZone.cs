using System;
using UnityEngine;

public class UpgradeZone : MonoBehaviour
{
    public Canvas icons;
    
    public void Upgrade(int upgrade)
    {
        if (upgrade == 0 || upgrade == 2)
        {
            if (Player.Instance.stats.qualitativeMaterial >= StaticGameVariables.qualitativeMaterialNeededForUpgrade)
            {
                Player.Instance.stats.qualitativeMaterial -= StaticGameVariables.qualitativeMaterialNeededForUpgrade;
                Upgrade(upgrade < 2);
            }

            return;
        }
        
        if (Player.Instance.stats.badQualityMaterial >= StaticGameVariables.badQualityMaterialNeededForUpgrade)
        {
            Player.Instance.stats.badQualityMaterial -= StaticGameVariables.badQualityMaterialNeededForUpgrade;
            Upgrade(upgrade < 2);
        }
    }
    
    private void Upgrade(bool healthOrStamina)
    {
        if (healthOrStamina)
        {
            Player.Instance.thisEntity.SetMaxHealth(Player.Instance.thisEntity.maxHealth + StaticGameVariables.healthGrade);
            Player.Instance.thisEntity.TakeHealthPercent(1f, null);

            return;
        }
                
        Player.Instance.stats.maxStamina += StaticGameVariables.staminaGrade;
        Player.Instance.stats.stamina = Player.Instance.stats.maxStamina;
        Player.Instance.stats.OnStaminaChanged?.Invoke(this, EventArgs.Empty);
    }
}
