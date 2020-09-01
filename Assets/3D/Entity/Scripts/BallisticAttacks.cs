using System.Collections;
using UnityEngine;
public class BallisticAttacks : EntityAttacks
{
    public float cast;
    public float radiusBallistic;
    public Animator animations;
    public ParticleSystem particle;
    public Transform target;

    public AnimationCurve curve;

    public Vector3 start;
    public Vector3 end;

    /*public Quaternion startRotation;
    public Quaternion endRotation;
    public Quaternion rotation;*/

    IEnumerator Parabola()
    {
        end = target.position;
        //FaceToFace();

        float time = 0f;
        while (particle.transform.position.y > end.y)
        {
            time += Time.deltaTime;
            Vector3 pos = Vector3.Lerp(start, end, time);
            //Quaternion rot = Quaternion.Slerp(rotation, endRotation, time);
            pos.y += curve.Evaluate(time);
            particle.transform.position = pos;
            //particle.transform.rotation = rot;

            yield return null;
        }

        RadiusAttack.RadiusDamage(gameObject, particle.transform.position, radiusBallistic, damage);

        particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        particle.transform.position = start;

        //particle.transform.rotation = startRotation;
    }

    public override void Start()
    {
        eyesPosition = new Vector3(transform.position.x, transform.position.y + 0.25f, transform.position.z);
        animations = GetComponent<Animator>();
        thisEntity = GetComponent<BaseEntity>();
        start = particle.transform.position;
        //startRotation = particle.transform.rotation;
    }

    public override void PrimaryAttack(GameObject target)
    {
        if (nextAttack > Time.time)
        {
            return;
        }

        this.target = target.transform;
        StartCoroutine(CastAttack());
        nextAttack = Time.time + fireRate + cast;
        if (animations != null)
            animations.SetInteger("Animation", 1);
        return;
    }

    IEnumerator CastAttack()
    {
        particle.Play();
        yield return new WaitForSeconds(cast);

        if (animations != null)
            animations.SetInteger("Animation", 0);
        Attack();
    }

    public void Attack()
    {
        StartCoroutine(Parabola());
        if (animations != null)
            animations.SetInteger("Animation", 1);
    }

    public override void SecondaryAttack()
    {
        return;
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    /*public void FaceToFace()
    {
        Vector3 direction = (target.position - particle.transform.position);
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        particle.transform.rotation = lookRotation * startRotation;
        rotation = particle.transform.rotation;
        endRotation = particle.transform.rotation;
        endRotation.eulerAngles = new Vector3(180f, endRotation.eulerAngles.y, endRotation.eulerAngles.z);
    }*/
}
