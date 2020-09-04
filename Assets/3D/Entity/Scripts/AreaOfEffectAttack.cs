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
        if (particle != null)
            particle.Play();
        yield return new WaitForSeconds(cast);

        if (animations != null)
            animations.SetInteger("Animation", 0);
        Attack();
    }

    public void Attack()
    {
        RadiusAttack.RadiusDamage(gameObject, transform.position, radius, damage);
        if (soundNumber != -1)
            sounds.Stop(soundsAttack[soundNumber]);
        soundNumber = GameDirector3D.PlayRandomSound(sounds, soundsAttack);
        if (animations != null)
            animations.SetInteger("Animation", 2);
    }

    public override void SecondaryAttack()
    {
        return;
    }
}
