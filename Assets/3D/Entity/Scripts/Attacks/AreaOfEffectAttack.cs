using System;
using System.Collections;
using System.Threading.Tasks;
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
            StartTimer();
            //StartCoroutine(CastAttack());
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

    public void StartTimer()
    {
        DoSomething(() => Attack());
    }

    public async void DoSomething(Action callback)
    {
        interrupted = false;
        await Task.Delay(500);
        if (!interrupted)
            callback();
    }

    public void Attack()
    {
        RadiusAttack.RadiusDamage(gameObject, transform.position, radius, damage, 1 << gameObject.layer);
        GameDirector3D.PlayRandomSound(sounds, soundsAttack, true);
    }

    public override void SecondaryAttack()
    {
        return;
    }
}
