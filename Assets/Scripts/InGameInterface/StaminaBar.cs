using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider bar;
    public TMP_Text staminaAmount;

    public Stats playerStats;

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
        staminaAmount.text = playerStats.stamina.ToString();
    }
}
