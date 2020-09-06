using System.Collections;
using UnityEngine;

public class CollidesAttack : EntityAttacks
{
    public float cast;
    public CollisionObject collides;

    public Animator animations;

    public AudioDirector sounds;
    public string[] soundsAttack;

    public override void Start()
    {
        collides = transform.GetChild(0).GetComponent<CollisionObject>();
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
            animations.SetInteger("Animation", 2);

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
        if (collides.objectCollision.Count > 0)
        {
            collides.objectCollision.RemoveAll(Collider => Collider == null);
            for (int i = 0; i < collides.objectCollision.Count; i++)
            {
                BaseEntity entity = collides.objectCollision[i].GetComponent<BaseEntity>();
                if (entity != null)
                {
                    entity.TakeDamage(damage, thisEntity);
                }
            }
        }
        GameDirector3D.PlayRandomSound(sounds, soundsAttack, true);
    }

    public override void SecondaryAttack()
    {
        return;
    }
}
