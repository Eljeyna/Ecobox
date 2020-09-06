using System.Collections;
using UnityEngine;
public class MeleeAttacks : EntityAttacks
{
    public float cast;
    public Animator animations;

    public AudioDirector sounds;
    public string[] soundsAttack;

    private int soundNumber;
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

        if (cast > 0f)
        {
            StartCoroutine(CastAttack());
            nextAttack = Time.time + fireRate;
            if (animations != null)
                animations.SetInteger("Animation", 2);
            return;
        }

        Attack();
        if (animations != null)
            animations.SetInteger("Animation", 2);
    }

    IEnumerator CastAttack()
    {
        yield return new WaitForSeconds(cast);
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

        if (soundNumber != -1)
            sounds.Stop(soundsAttack[soundNumber]);
        soundNumber = GameDirector3D.PlayRandomSound(sounds, soundsAttack);
        nextAttack = Time.time + fireRate;
    }

    public override void SecondaryAttack()
    {
        return;
    }
}
