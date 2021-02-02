using UnityEngine;

[CreateAssetMenu(fileName = "GunData", menuName = "ScriptableObjects/GunData")]
public class GunData : ScriptableObject
{
    public WeaponDamageType damageType;
    public float damage;
    public float radius;
    public float range;
    public int delay;
    public float fireRatePrimary;
    public float fireRateSecondary;
    public float impactForce;

    public float reloadTime;

    public bool autoreload = true;

    public int maxClip;
    public int maxAmmo;
}
