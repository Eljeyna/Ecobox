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
        BaseCommon basePlayer = Player.Instance.thisEntity;
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
        bar.maxValue = Player.Instance.thisEntity.maxHealth;
        bar.value = Player.Instance.thisEntity.health;

        sb.Append((int)Player.Instance.thisEntity.health);
        healthAmount.text = sb.ToString();

        sb.Clear();
        sb.Append((int)Player.Instance.thisEntity.maxHealth);
        healthAmountMax.text = sb.ToString();

        sb.Clear();
    }
}
