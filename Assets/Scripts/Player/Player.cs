using UnityEngine;
using UnityEngine.InputSystem;

public enum State
{
    Normal  = 0,
    Dash    = 1,
    Stun    = 2,
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
    public Dash dash;
    public Animator animations;
    public Rigidbody2D rb2d;
    public Camera cam;
    public BasePlayer thisPlayer;
    public Stats stats;
    public Gun weapon;

    [Space(10)]
    public State state;

    [HideInInspector] public Vector3 moving;
    [HideInInspector] public Vector2 moveVelocity;
    [HideInInspector] public Vector3 dashDirection;

    private Vector2 mousePos;

    private bool attack;

    private void Awake()
    {
        Instance = this;

        controls = new NewInputSystem();

        controls.Player.Movement.performed += movementEvent => moving = movementEvent.ReadValue<Vector2>();
        controls.Player.Movement.canceled += movementEvent => moving = Vector3.zero;

        /*controls.Player.Attack.performed += attackEvent => attack = true;
        controls.Player.Attack.canceled += attackEvent => attack = false;*/

        StaticGameVariables.InitializeAwake();
    }

    private async void Start()
    {
        inventory = new Inventory();
        inventoryUI.SetInventory(inventory);

        StaticGameVariables.Initialize();
        
        /* Test */
        thisPlayer.TakeDamagePercent(0.5f, 0, null);

        GameObject test = await Pool.Instance.GetFromPoolAsync(0);
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
            case State.Dash:
                StateDash();
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

        if (weapon && attack && weapon.nextAttack <= Time.time)
        {
            if (weapon.clip == 0)
            {
                weapon.fireWhenEmpty = true;
            }

            weapon.PrimaryAttack();
            /*CinemachineShaker.Instance.enabled = true;
            CinemachineShaker.Instance.ShakeSmooth(shakeForce, weapon.gunData.fireRatePrimary);*/
        }
    }

    private void StateDash()
    {
        if (dash.nextDash <= Time.time)
        {
            rb2d.velocity = Vector2.zero;
            state = State.Normal;
        }
        else
        {
            float dashSpeed = dash.dashSpeed.Evaluate(dash.dashEvaluateTime);
            rb2d.velocity = dashDirection * dashSpeed;
            dash.dashEvaluateTime += Time.deltaTime;
        }
    }

    private void StateStun()
    {
        return;
    }

    private void OnDash()
    {
        if (stats.stamina > dash.staminaCost && dash.nextDashTime <= Time.time)
        {
            stats.stamina -= dash.staminaCost;
            dash.dashEvaluateTime = 0f;
            dashDirection = moving == Vector3.zero ? transform.up : moving.normalized;
            dash.enabled = true;
            dash.Use();
            state = State.Dash;
        }
    }

    private void OnReload()
    {
        if (weapon && !weapon.reloading)
        {
            weapon.Reload();
        }
    }

    private void OnChangeLanguage()
    {
        StaticGameVariables.ChangeLanguage(Random.Range(0, 2));
    }

    private void OnInventory()
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

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
