using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider bar;
    public TMP_Text healthAmount;
    public TMP_Text healthAmountMax;

    public BasePlayer basePlayer;

    private void Awake()
    {
        basePlayer.OnHealthChanged += OnHealthChanged;
    }

    public void OnHealthChanged(object sender, System.EventArgs e)
    {
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        bar.maxValue = basePlayer.maxHealth;
        bar.value = basePlayer.health;
        healthAmount.text = basePlayer.health.ToString();
        healthAmountMax.text = basePlayer.maxHealth.ToString();
    }
}
