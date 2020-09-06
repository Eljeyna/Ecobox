using System.Collections;
using UnityEngine;

public class AreaOfEffectAttack : EntityAttacks
{
    public float cast;
    public float radius;
    public Animator animations;
    public ParticleSystem particle;

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
            castCoroutine = StartCoroutine(CastAttack());
            return;
        }

        Attack();
    }

    public override void StopCastAttackCoroutine()
    {
        if (castCoroutine != null)
        {
            StopCoroutine(castCoroutine);
            castCoroutine = null;
        }
    }

    IEnumerator CastAttack()
    {
        if (particle != null)
            particle.Play();
        yield return new WaitForSeconds(cast);
        Attack();
    }

    public void Attack()
    {
        castCoroutine = null;
        RadiusAttack.RadiusDamage(gameObject, transform.position, radius, damage, 1 << gameObject.layer);
        GameDirector3D.PlayRandomSound(sounds, soundsAttack, true);
    }

    public override void SecondaryAttack()
    {
        return;
    }
}
