using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 4f;
    public FixedJoystick joystick;
    public LayerMask LAYER_MASK;

    [HideInInspector] public Camera cam;
    [HideInInspector] public Animator animations;
    [HideInInspector] public Collider playerCollider;
    [HideInInspector] public RaycastHit hit;
    [HideInInspector] public Ray ray;
    [HideInInspector] public EntityAttacks attack;
    [HideInInspector] public GameObject target;
    [HideInInspector] public BasePlayer thisPlayer;
    [HideInInspector] public float nextWait;
    [HideInInspector] public Rigidbody rigidBody;
    [HideInInspector] public Vector3 moveVelocity;

    public AudioDirector sounds;
    public string[] soundsIdle;
    public string[] soundsDeath;

#if UNITY_ANDROID
    private Touch lastTouch;
#endif

    private void Start()
    {
        cam = Camera.main;
        rigidBody = GetComponent<Rigidbody>();
        animations = GetComponent<Animator>();
        playerCollider = GetComponent<Collider>();
        attack = GetComponent<EntityAttacks>();
        sounds = GameDirector3D.GetAudioDirector();
        thisPlayer = GetComponent<BasePlayer>();
        sounds.Play("Ambient");
    }

    private void FixedUpdate()
    {
        rigidBody.MovePosition(rigidBody.position + moveVelocity * Time.fixedDeltaTime);
    }

    private void Update()
    {
        if (thisPlayer.flagDeath)
        {
            animations.Play("Death");
            GameDirector3D.DefeatGame();
            this.enabled = false;
            return;
        }
        else
        {
            CharacterControl();
        }
    }

    public void CharacterControl()
    {
        if (nextWait > Time.time)
        {
            return;
        }

        if (thisPlayer.attacker != null)
        {
            moveVelocity = Vector3.zero;
            thisPlayer.attacker = null;
            nextWait = Time.time + 0.5f;
            animations.Play("Hit");
            return;
        }

        Vector3 moving;
#if UNITY_ANDROID
        moving = new Vector3(joystick.Direction.x, 0f, joystick.Direction.y);
#else
        moving = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
#endif

        moveVelocity = moving.normalized * speed;
        moveVelocity = cam.transform.TransformDirection(moveVelocity);
        moveVelocity = Vector3.ProjectOnPlane(moveVelocity, Vector3.up);

#if !UNITY_ANDROID
        Ray cameraRay = cam.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(cameraRay, out float rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
#endif

        if (moving.x != 0f || moving.z != 0f)
        {
            animations.SetInteger("Animation", 1);
#if UNITY_ANDROID
            FaceToFace(rigidBody.position + moveVelocity);
#endif
        }
        else
        {
            animations.SetInteger("Animation", 0);
        }

#if UNITY_ANDROID
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                if (lastTouch.phase != TouchPhase.Moved && lastTouch.phase != TouchPhase.Stationary)
                {
                    Ray cameraRay = cam.ScreenPointToRay(touch.deltaPosition);
                    Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

                    if (groundPlane.Raycast(cameraRay, out float rayLength))
                    {
                        Vector3 pointToLook = cameraRay.GetPoint(rayLength);
                        transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
                    }

                    Attack();
                }

                lastTouch = touch;
            }
        }
#else
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
#endif
    }

    public void Attack()
    {
        if (attack.nextAttack <= Time.time)
        {
            moveVelocity = Vector3.zero;
            nextWait = Time.time + attack.fireRate / 2;
            attack.PrimaryAttack(null);
        }
    }

    public Quaternion FaceToFace(Vector3 target)
    {
        Vector3 direction = (target - transform.position);
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = lookRotation;

        return lookRotation;
    }

#if UNITY_EDITOR
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
    {
            thisPlayer.TakeHealth(1, null);
            sounds.Play("Healing");
            Destroy(other.gameObject);
        }
    }
#endif
}
