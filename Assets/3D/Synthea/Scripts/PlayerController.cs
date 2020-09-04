using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 2f;
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
    [HideInInspector] public float waitingTime;
    [HideInInspector] public Rigidbody rigidBody;
    [HideInInspector] public Vector3 moveVelocity;

    public AudioDirector sounds;
    public string[] soundsIdle;
    public string[] soundsDeath;

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
        //rigidBody.MovePosition(rigidBody.position + moveVelocity * Time.fixedDeltaTime);
    }

    private void Update()
    {
        if (thisPlayer.flagDeath)
        {
            playerCollider.enabled = false;
            animations.Play("Death");
            GameDirector3D.DefeatGame();
            this.enabled = false;
            return;
        }

        if (waitingTime > Time.time)
            return;

        Vector3 moving;
        if (SystemInfo.deviceType == DeviceType.Desktop)
            moving = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        else
            moving = new Vector3(joystick.Direction.x, 0f, joystick.Direction.y);

        moveVelocity = moving.normalized * speed;
        moveVelocity = cam.transform.TransformDirection(moveVelocity);
        moveVelocity = Vector3.ProjectOnPlane(moveVelocity, Vector3.up);

        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            //transform.Rotate(new Vector3(Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0f));
            Ray cameraRay = cam.ScreenPointToRay(Input.mousePosition);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

            if (groundPlane.Raycast(cameraRay, out float rayLength))
            {
                Vector3 pointToLook = cameraRay.GetPoint(rayLength);
                Debug.DrawLine(cameraRay.origin, pointToLook, Color.cyan);

                transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
            }
        }


        if (moving.x != 0f || moving.z != 0f)
        {
            animations.SetInteger("Animation", 1);
            if (SystemInfo.deviceType != DeviceType.Desktop)
                FaceToFace(rigidBody.position + moving);
        }
        else
        {
            animations.SetInteger("Animation", 0);
        }

        if (thisPlayer.attacker != null)
        {
            thisPlayer.attacker = null;
            waitingTime = Time.time + 0.5f;
            animations.Play("Hit");
            return;
        }

        if (SystemInfo.deviceType == DeviceType.Desktop && Input.GetMouseButtonDown(0))
        {
            Attack();
        }

    }

    public void Attack()
    {
        if (attack.nextAttack <= Time.time)
        {
            //attack.PrimaryAttack(hit.collider.gameObject);
            attack.PrimaryAttack(null);
            waitingTime = Time.time + attack.fireRate / 2;
        }
    }

    public Quaternion FaceToFace(Vector3 target)
    {
        Vector3 direction = (target - transform.position);
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = lookRotation;

        return lookRotation;
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
