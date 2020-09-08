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

    private NewInputSystem controls;

    private Vector3 moving;
    private bool attacking;

    private void Awake()
    {
        controls = new NewInputSystem();
    }

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
        sounds = GetComponent<AudioDirector>();

        controls.Player.Movement.performed += movementEvent => moving = movementEvent.ReadValue<Vector2>();
        controls.Player.Movement.canceled += movementEvent => moving = Vector3.zero;
        controls.Player.Attack.performed += attackEvent => attacking = true;
        controls.Player.Attack.canceled += attackEvent => attacking = false;
    }

    private void FixedUpdate()
    {
        if (moveVelocity != Vector3.zero)
            rigidBody.MovePosition(rigidBody.position + moveVelocity * Time.fixedDeltaTime);
    }

    private void Update()
    {
        if (thisPlayer.flagDeath)
        {
            controls.Disable();
            attack.StopCastAttackCoroutine();
            attack.enabled = false;
            animations.Play("Death");
            GameDirector3D.DefeatGame();
            this.enabled = false;
            return;
        }

        CharacterControl();
    }

    public void CharacterControl()
    {
        if (thisPlayer.attacker != null)
        {
            if (attack.interruptAttack)
                attack.StopCastAttackCoroutine();
            thisPlayer.attacker = null;
            nextWait = Time.time + 0.5f;
            moveVelocity = Vector3.zero;
            animations.Play("Hit");
            return;
        }

        if (nextWait > Time.time)
        {
            return;
        }

#if UNITY_ANDROID || UNITY_IOS
        if (joystick.Direction != Vector2.zero)
            moving = new Vector3(joystick.Direction.x, 0f, joystick.Direction.y);
#endif

        moveVelocity = moving.normalized * speed;
        moveVelocity = cam.transform.TransformDirection(moveVelocity);
        moveVelocity = Vector3.ProjectOnPlane(moveVelocity, Vector3.up);
        moveVelocity.y = 0f;

        if (moving != Vector3.zero)
        {
            animations.SetInteger("Animation", 1);
            FaceToFace(rigidBody.position + moveVelocity);
        }
        else
        {
            animations.SetInteger("Animation", 0);
        }

        if (attacking)
        {
            attacking = false;
            Attack();
        }

#if UNITY_ANDROID || UNITY_IOS
        if (joystick.Direction != Vector2.zero)
            moving = Vector3.zero;
#endif
    }

    public void Attack()
    {
        if (attack.nextAttack <= Time.time)
        {
            /*Ray cameraRay = cam.ScreenPointToRay(Pointer.current.position.ReadValue());
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

            if (groundPlane.Raycast(cameraRay, out float rayLength))
            {
                Vector3 pointToLook = cameraRay.GetPoint(rayLength);
                transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
            }*/

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

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
    
    void OnTriggerEnter(Collider other)
    {
        BaseTag pickUpEntity = other.GetComponent<BaseTag>();
        if (pickUpEntity != null && (pickUpEntity.entityTag & Tags.EntityTags.FL_PICKUP) != 0)
        {
            thisPlayer.TakeHealth(1, null);
            sounds.Play("Healing");
            Destroy(other.gameObject);
        }
    }
}
