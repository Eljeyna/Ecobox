using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    public BulletData bulletData;
    public BaseEntity owner;
    public BaseTag baseTag;
    [HideInInspector] public float nextFade;
}
