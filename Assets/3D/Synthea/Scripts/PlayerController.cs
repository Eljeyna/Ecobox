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

    private void Start()
    {
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        animations = GetComponent<Animator>();
        playerCollider = GetComponent<Collider>();
        attack = GetComponent<EntityAttacks>();
    }

    private void Update()
    {
        if (hit.point != Vector3.zero && agent.remainingDistance <= agent.stoppingDistance)
        {
            animations.SetInteger("Animations", 0);
        }

        if (Input.GetMouseButtonDown(0))
        {
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 10f))
            {
                transform.LookAt(hit.point);
                if (hit.collider && !hit.collider.isTrigger && hit.collider.CompareTag("Enemies"))
                {
                    BaseEntity entity = hit.collider.gameObject.GetComponent<BaseEntity>();
                    if (entity != null)
                    {
                        attack.PrimaryAttack(hit.collider.gameObject);
                        return;
                    }
                }

                agent.SetDestination(hit.point);
                animations.SetInteger("Animations", 1);
            }
        }
    }

    public void FaceToFace(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = lookRotation;
    }
}
