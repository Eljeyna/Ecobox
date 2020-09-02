using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    public LayerMask LAYER_MASK;

    [HideInInspector] public Camera cam;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator animations;
    [HideInInspector] public Collider playerCollider;
    [HideInInspector] public RaycastHit hit;
    [HideInInspector] public Ray ray;
    [HideInInspector] public EntityAttacks attack;
    [HideInInspector] public GameObject target;
    [HideInInspector] public BasePlayer thisPlayer;
    [HideInInspector] public float waitingTime;

    public AudioDirector sounds;
    public string[] soundsIdle;
    public string[] soundsDeath;

    private void Start()
    {
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        animations = GetComponent<Animator>();
        playerCollider = GetComponent<Collider>();
        attack = GetComponent<EntityAttacks>();
        sounds = GameDirector3D.GetAudioDirector();
        thisPlayer = GetComponent<BasePlayer>();
        sounds.Play("Ambient");
    }

    private void Update()
    {
        if (thisPlayer.flagDeath)
        {
            agent.enabled = false;
            playerCollider.enabled = false;
            animations.Play("Death");
            GameDirector3D.DefeatGame();
            this.enabled = false;
            return;
        }

        if (waitingTime > Time.time)
            return;

        if (thisPlayer.attacker != null)
        {
            thisPlayer.attacker = null;
            agent.SetDestination(transform.position);
            waitingTime = Time.time + 0.5f;
            animations.Play("Hit");
            return;
        }

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            animations.SetInteger("Animation", 0);
        }

        if (Input.GetMouseButtonDown(0))
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 10f))
            {
                transform.LookAt(hit.point);
                agent.SetDestination(hit.point);
                animations.SetInteger("Animation", 1);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Attack();
        }

    }

    public void Attack()
    {
        GameObject enemy = RadiusAttack.FindEnemy(gameObject, transform.position, attack.attackRange);
        if (enemy != null)
        {
            agent.SetDestination(transform.position);
            transform.LookAt(enemy.transform);
            if (attack.nextAttack <= Time.time)
            {
                attack.PrimaryAttack(hit.collider.gameObject);
                waitingTime = Time.time + attack.fireRate / 2;
            }
        }
    }

    public void FaceToFace(Vector3 target)
    {
        Vector3 direction = (target - transform.position);
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = lookRotation;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
    {
            thisPlayer.TakeHealth(1, null);
            sounds.Play("Healing");
            Destroy(other.gameObject);
        }
    }
}
