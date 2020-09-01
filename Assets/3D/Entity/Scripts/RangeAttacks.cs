using System.Collections;
using UnityEngine;
public class RangeAttacks : EntityAttacks
{
    public float cast;
    public Animator animations;

    public override void Start()
    {
        eyesPosition = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
        animations = GetComponent<Animator>();
        thisEntity = GetComponent<BaseEntity>();
    }

    public override void PrimaryAttack(GameObject target)
    {
        if (nextAttack > Time.time)
        {
            return;
        }

        eyesPosition = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);

        if (cast > 0f)
        {
            StartCoroutine(CastAttack());
            nextAttack = Time.time + cast;
            if (animations != null)
                animations.SetInteger("Animation", 1);
            return;
        }

        Attack();
    }

    IEnumerator CastAttack()
    {
        yield return new WaitForSeconds(cast);

        if (animations != null)
            animations.SetInteger("Animation", 0);
        Attack();
    }

    public void Attack()
    {
        RaycastHit hit;
        eyesPosition = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
        if (Physics.Raycast(eyesPosition, transform.forward, out hit, attackRange))
        {
            BaseEntity entity = hit.transform.GetComponent<BaseEntity>();
            if (entity != null)
            {
                entity.TakeDamage(damage, thisEntity);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce, ForceMode.Impulse);
            }
        }

        nextAttack = Time.time + fireRate;
        if (cast > 0f && animations != null)
            animations.SetInteger("Animation", 1);
    }

    public override void SecondaryAttack()
    {
        return;
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(eyesPosition, transform.forward * attackRange, Color.yellow);
    }
}
