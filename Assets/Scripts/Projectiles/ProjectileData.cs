using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileData", menuName = "ScriptableObjects/ProjectileData")]
public class ProjectileData : ScriptableObject
{
    public int index;
    public WeaponDamageType damageType;
    public float damage;
    public float speed;
    public float timeFade;
    public float radius;
}
