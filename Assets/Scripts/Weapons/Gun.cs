using UnityEngine;

public enum WeaponDamageType
{
    Physical    = 0,   // Common Damage
    Chemical    = 1,   // Poison
    EMP         = 2,   // Electricity
    Thermal     = 3,   // Cold and Hot
}

public abstract class Gun : MonoBehaviour
{
    public GunData gunData;

    [HideInInspector] public float nextAttack;
    public bool reloading = false;
    public bool fireWhenEmpty = false;

    public int clip;
    public int ammo;

    public abstract void PrimaryAttack();
    public abstract void SecondaryAttack();
    public abstract bool Reload();
}
