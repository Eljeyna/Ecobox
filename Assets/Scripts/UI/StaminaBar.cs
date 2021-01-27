using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Text;

public class StaminaBar : MonoBehaviour
{
    public Slider bar;
    public TMP_Text staminaAmount;

    public Stats playerStats;

    private StringBuilder sb = new StringBuilder(32);

    private void Awake()
    {
        playerStats.OnStaminaChanged += OnStaminaChanged;
    }

    private void Start()
    {
        UpdateStaminaBar();
    }

    public void OnStaminaChanged(object sender, System.EventArgs e)
    {
        UpdateStaminaBar();
    }

    public void UpdateStaminaBar()
    {
        bar.maxValue = playerStats.maxStamina;
        bar.value = playerStats.stamina;

        sb.Append(playerStats.stamina);
        staminaAmount.text = sb.ToString();

        sb.Clear();
    }
}
