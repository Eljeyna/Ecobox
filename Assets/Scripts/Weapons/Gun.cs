using UnityEngine;

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
