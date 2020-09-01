using UnityEngine;
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

    private void Start()
    {
        entityAttacks = GetComponent<EntityAttacks>();
        agent = GetComponent<NavMeshAgent>();
        animations = GetComponent<Animator>();
        triggerEnemy = GetComponent<BoxCollider>();
        thisEntity = GetComponent<BaseEnemy>();
        target = GameDirector3D.GetPlayer();
    }

    void Update()
    {
        if (thisEntity.flagDeath)
        {
            if (agent != null)
                agent.enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            this.enabled = false;
            //animations.SetInteger("Animation", 4);
            if (animations != null)
                animations.Play("Death");
            return;
        }

        if (nextWait > Time.time)
            return;

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

        if (entityAttacks.nextAttack - entityAttacks.fireRate / 4 <= Time.time)
        {
            if (animations != null)
                animations.SetInteger("Animation", 2);
        }

        if (distance <= entityAttacks.attackRange)
        {
            PrimaryAttack();
        }
    }

    public void PrimaryAttack()
    {
        waitingTime = Time.time + 1f;
        entityAttacks.PrimaryAttack(target.gameObject);
    }

    public void SecondaryAttack()
    {
        waitingTime = Time.time + 1f;
        entityAttacks.SecondaryAttack();
    }

    public void FaceToFace()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        triggerEnemy.enabled = false;
        target = other.gameObject.transform;
        waitingTime = Time.time + 0.5f;
    }
}
