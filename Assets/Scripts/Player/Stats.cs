using UnityEngine;

public class Stats : MonoBehaviour
{
    [Space(10)]
    public int maxStamina = 50;
    public int stamina;
    public float staminaRegen = 0.25f;

    public int level = 1;
    public int exp;
    public int talentPoints;

    public float weight = 100f;

    [Space(10)]
    public int strength = 3;
    public int agility = 3;
    public int intelligence = 3;

    [Space(10)]
    public int money;

    private float nextStaminaRegen;

    private void Awake()
    {
        stamina = maxStamina;
    }

    private void Update()
    {
        if (stamina < maxStamina && nextStaminaRegen <= Time.time)
        {
            stamina++;
            nextStaminaRegen = Time.time + staminaRegen;
        }
    }
}
