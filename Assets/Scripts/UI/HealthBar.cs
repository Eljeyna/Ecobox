using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text;

public class HealthBar : MonoBehaviour
{
    public Slider bar;
    public TMP_Text healthAmount;
    public TMP_Text healthAmountMax;

    public BasePlayer basePlayer;

    private StringBuilder sb = new StringBuilder();

    private void Awake()
    {
        basePlayer.OnHealthChanged += OnHealthChanged;
    }

    private void Start()
    {
        UpdateHealthBar();
    }

    public void OnHealthChanged(object sender, System.EventArgs e)
    {
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        bar.maxValue = basePlayer.maxHealth;
        bar.value = basePlayer.health;

        sb.Append(basePlayer.health);
        healthAmount.text = sb.ToString();

        sb.Clear();
        sb.Append(basePlayer.maxHealth);
        healthAmountMax.text = sb.ToString();

        sb.Clear();
    }
}
