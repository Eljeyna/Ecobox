﻿using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public EntityAttacks entityAttacks;
    public float waitingTime;
    public bool allowRotation = true;
    public LayerMask ENTITY_MASK;
    public Transform target;
    public float nextWait;

    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator animations;
    [HideInInspector] public BoxCollider triggerEnemy;
    [HideInInspector] public BaseEntity thisEntity;

    public AudioDirector sounds;
    public string[] soundsIdle;
    public string[] soundsDeath;

    private void Start()
    {
        entityAttacks = GetComponent<EntityAttacks>();
        agent = GetComponent<NavMeshAgent>();
        animations = GetComponent<Animator>();
        triggerEnemy = GetComponent<BoxCollider>();
        thisEntity = GetComponent<BaseEnemy>();
        target = GameDirector3D.GetPlayer();
        sounds = GetComponent<AudioDirector>();
    }

    void Update()
    {
        if (thisEntity.flagDeath)
        {
            if (agent != null)
                agent.enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            this.enabled = false;
            if (animations != null)
                animations.Play("Death");
            GameDirector3D.PlayRandomSound(sounds, soundsDeath);

            Spawner3D spawner = transform.parent.GetComponent<Spawner3D>();
            if (spawner != null)
            {
                spawner.currentWaveCount--;
                spawner.waveEntities.Remove(gameObject);
            }

            Destroy(gameObject, 2f);
            return;
        }
        else
        {
            CharacterControl();
        }
    }

    public void CharacterControl()
    {
        GameDirector3D.PlayRandomSound(sounds, soundsIdle);

        if (nextWait > Time.time)
        {
            return;
        }

        if (thisEntity.attacker != null)
        {
            thisEntity.attacker = null;
            nextWait = Time.time + 0.5f;
            if (agent != null)
                agent.SetDestination(transform.position);
            if (animations != null)
            {
                animations.SetInteger("Animation", 0);
                animations.Play("Hit");
            }
            return;
        }

        if (target == null)
        {
            if (thisEntity.attacker != null)
            {
                target = thisEntity.attacker.gameObject.transform;
            }
            else
            {
                nextWait = Time.time + waitingTime;
                if (animations != null)
                    animations.SetInteger("Animation", 0);
                return;
            }
        }

        float distance = Vector3.Distance(target.position, transform.position);

        if (agent != null)
            agent.SetDestination(target.position);

        if (allowRotation)
            FaceToFace();

        if (animations != null)
        {
            if (entityAttacks.nextAttack - entityAttacks.fireRate / 4 <= Time.time)
            {
                animations.SetInteger("Animation", 2);
            }
        }

        if (distance <= entityAttacks.attackRange)
        {
            if (agent != null)
                agent.SetDestination(transform.position);
            PrimaryAttack();
        }
    }

    public void PrimaryAttack()
    {
        nextWait = Time.time + 1f;
        entityAttacks.PrimaryAttack(target.gameObject);
    }

    public void SecondaryAttack()
    {
        nextWait = Time.time + 1f;
        entityAttacks.SecondaryAttack();
    }

    public void FaceToFace()
    {
        /*Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);*/

        Vector3 direction = (target.position - transform.position);
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = lookRotation;
    }

    /*private void OnTriggerEnter(Collider other)
    {
        triggerEnemy.enabled = false;
        target = other.gameObject.transform;
        nextWait = Time.time + 0.5f;
    }*/
}
