using UnityEngine;
using UnityEngine.UI;

public class HealthGUI : MonoBehaviour
{
    public BasePlayer player;
    public Animator heart;
    private Image img;

    private void Awake()
    {
        img = GetComponent<Image>();
    }

    private void Start()
    {
        ChangeText();
    }

    public void SetEntity(BasePlayer entity)
    {
        player = entity;
        ChangeText();
    }

    public void ChangeText()
    {
        img.fillAmount = player.health / player.maxHealth;
    }

    public void ChangeText(float amount)
    {
        img.fillAmount = amount / player.maxHealth;
        Heart();
    }

    public void Heart()
    {
        heart.Play("Damaged");
    }
}
