using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text;

public class HealthBar : MonoBehaviour
{
    public Slider bar;
    public TMP_Text healthAmount;
    public TMP_Text healthAmountMax;

    private StringBuilder sb = new StringBuilder();

    private void Awake()
    {
        Player.Instance.thisPlayer.OnHealthChanged += OnHealthChanged;
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
        bar.maxValue = Player.Instance.thisPlayer.maxHealth;
        bar.value = Player.Instance.thisPlayer.health;

        sb.Append(Player.Instance.thisPlayer.health);
        healthAmount.text = sb.ToString();

        sb.Clear();
        sb.Append(Player.Instance.thisPlayer.maxHealth);
        healthAmountMax.text = sb.ToString();

        sb.Clear();
    }
}
