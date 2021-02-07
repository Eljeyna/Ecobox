using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    public readonly BulletData bulletData;
    public BaseEntity owner;
    public Tags baseTag;
    [HideInInspector] public float nextFade;
}
