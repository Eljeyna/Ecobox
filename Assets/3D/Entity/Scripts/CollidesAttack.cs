using System.Collections;
using UnityEngine;

public class CollidesAttack : EntityAttacks
{
    public float cast;
    public CollisionObject collides;

    public Animator animations;

    public AudioDirector sounds;
    public string[] soundsAttack;

    private int soundNumber;

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
        if (collides.objectCollision.Count > 0)
        {
            for (int i = 0; i < collides.objectCollision.Count; i++)
            {
                if (collides.objectCollision[i] == null)
                {
                    collides.objectCollision.Remove(collides.objectCollision[i]);
                }
                else
                {
                    BaseEntity entity = collides.objectCollision[i].GetComponent<BaseEntity>();
                    if (entity != null)
                    {
                        entity.TakeDamage(damage, thisEntity);
                    }
                }
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
