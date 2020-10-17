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
            if (castCoroutine != null)
                StopCoroutine(castCoroutine);
            castCoroutine = StartCoroutine(CastAttack(cast));
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

    IEnumerator CastAttack(float remainingTime)
    {
        yield return new WaitForSeconds(cast);
        Attack();
    }

    public void Attack()
    {
        castCoroutine = null;
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
