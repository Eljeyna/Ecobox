using UnityEngine;

public abstract class EntityAttacks : MonoBehaviour
{
    public float damage;
    public float attackRange;
    public float fireRate;
    public bool interuptAttack = false;
    public Vector3 eyesPosition;
    public BaseEntity thisEntity;
    public float impactForce;
    public float nextAttack;

    public abstract void Start();
    public abstract void PrimaryAttack(GameObject target);
    public abstract void SecondaryAttack();
}
