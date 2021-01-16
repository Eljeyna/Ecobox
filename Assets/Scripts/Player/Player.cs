using UnityEngine;
using UnityEngine.InputSystem;

public enum State
{
    Normal = 0,
    Stun = 1,
}

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [HideInInspector] public Inventory inventory;

    private NewInputSystem controls;

    public float speed = 4f;
    public float shakeForce = 2f;

    [Space(10)]
    [SerializeField] private InventoryUI inventoryUI;
    public Animator animations;
    public Rigidbody2D rb2d;
    public Camera cam;
    public BasePlayer thisPlayer;
    //public Gun weapon;

    [Space(10)]
    public State state;

    [HideInInspector] public Vector3 moving;
    [HideInInspector] public Vector2 moveVelocity;
    [HideInInspector] public Vector3 dashDirection;

    private Vector2 mousePos;

    private bool attack;

    private void Awake()
    {
        controls = new NewInputSystem();

        controls.Player.Movement.performed += movementEvent => moving = movementEvent.ReadValue<Vector2>();
        controls.Player.Movement.canceled += movementEvent => moving = Vector3.zero;

        /*controls.Player.Attack.performed += attackEvent => attack = true;
        controls.Player.Attack.canceled += attackEvent => attack = false;*/
    }

    private void Start()
    {
        Instance = this;

        inventory = new Inventory();
        inventoryUI.SetInventory(inventory);

        controls.Player.Inventory.performed += OpenInventory_performed;
        controls.Player.ChangeLanguage.performed += ChangeLanguage_performed;

        StaticGameVariables.Initialize();
    }

    private void FixedUpdate()
    {
        if (state == State.Normal)
        {
            if (moveVelocity != Vector2.zero)
            {
                rb2d.MovePosition(rb2d.position + moveVelocity * Time.fixedDeltaTime);
            }
        }
    }

    private void Update()
    {
        switch (state)
        {
            case State.Normal:
                StateNormal();
                break;
            case State.Stun:
                StateStun();
                break;
        }
    }

    private void StateNormal()
    {
        mousePos = cam.ScreenToWorldPoint(Pointer.current.position.ReadValue());

        moveVelocity = moving.normalized * speed;

        /*if (attack && weapon.nextAttack <= Time.time)
        {
            if (weapon.clip == 0)
            {
                weapon.fireWhenEmpty = true;
            }

            weapon.PrimaryAttack();
            CinemachineShaker.Instance.enabled = true;
            CinemachineShaker.Instance.ShakeSmooth(shakeForce, weapon.gunData.fireRatePrimary);
        }*/
    }

    private void StateStun()
    {
        return;
    }

    /*private void OnReload()
    {
        if (!weapon.reloading)
        {
            weapon.Reload();
        }
    }*/

    private void ChangeLanguage_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        StaticGameVariables.ChangeLanguage(Random.Range(0, 2));
    }

    private void OpenInventory_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (StaticGameVariables.inventoryCanvas.isActiveAndEnabled)
        {
            StaticGameVariables.HideInventory();
        }
        else
        {
            StaticGameVariables.OpenInventory();
        }
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 30) // Items
        {
            ItemWorld itemWorld = collision.GetComponent<ItemWorld>();
            if (itemWorld)
            {
                inventory.AddItem(itemWorld.item);
                Destroy(collision.gameObject);
            }
        }
    }
}
