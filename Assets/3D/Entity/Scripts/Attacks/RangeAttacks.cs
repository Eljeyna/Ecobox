﻿using System.Collections;
using UnityEngine;
public class RangeAttacks : EntityAttacks
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

        eyesPosition = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
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
        yield return new WaitForSeconds(cast);
        Attack();
    }

    public void Attack()
    {
        castCoroutine = null;
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

    private void OnDrawGizmos()
    {
        Debug.DrawRay(eyesPosition, transform.forward * attackRange, Color.yellow);
    }
}
