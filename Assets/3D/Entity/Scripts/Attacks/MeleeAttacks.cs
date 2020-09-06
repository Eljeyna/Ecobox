using System.Collections;
using UnityEngine;
public class MeleeAttacks : EntityAttacks
{
    public float cast;
    public Animator animations;

    public AudioDirector sounds;
    public string[] soundsAttack;
    public override void Start()
    {
        eyesPosition = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
        animations = GetComponent<Animator>();
        thisEntity = GetComponent<BaseEntity>();
        sounds = GetComponent<AudioDirector>();
    }

    public override void PrimaryAttack(GameObject target)
    {
        if (nextAttack > Time.time)
        {
            return;
        }

        nextAttack = Time.time + fireRate + cast;
        if (animations != null)
            animations.SetInteger("Animation", 1);

        if (cast > 0f)
        {
            StartCoroutine(CastAttack());
            return;
        }

        Attack();
    }

    IEnumerator CastAttack()
    {
        float remainingTime = Time.time + cast;
        while (remainingTime > Time.time)
        {
            yield return null;
            if (interruptAttack && thisEntity.attacker != null)
            {
                yield break;
            }
        }
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
        GameDirector3D.PlayRandomSound(sounds, soundsAttack, true);
    }

    public override void SecondaryAttack()
    {
        return;
    }
}
