using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public ProjectileData bulletData;
    public BaseEntity owner;
    public BaseTag baseTag;
    [HideInInspector] public float nextFade;
}
