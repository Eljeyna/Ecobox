using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider bar;
    public TMP_Text staminaAmount;

    private void Awake()
    {
        Player.Instance.stats.OnStaminaChanged += OnStaminaChanged;
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
        bar.maxValue = Player.Instance.stats.maxStamina;
        bar.value = Player.Instance.stats.stamina;
        staminaAmount.text = Player.Instance.stats.stamina.ToString();
    }
}
