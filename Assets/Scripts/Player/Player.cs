using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Cinemachine;

#if UNITY_ANDROID || UNITY_IOS
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
#endif

public enum EntityState
{
    None    = 0,
    Normal  = 1,
    Dash    = 2,
    Stun    = 3,
}

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [HideInInspector] public Inventory inventory;

    public float speed = 8f;
    public float speedSlow;
    public float shakeForce = 2f;

    public float camMaxSize = 20f;
    public float camMinSize = 8f;

    [Space(10)]
    [SerializeField] private InventoryUI inventoryUI;
    public Dash dash;
    public Animator animations;
    public Rigidbody2D rb;
    public CinemachineVirtualCamera cam;
    public BasePlayer thisPlayer;
    public Stats stats;
    public Gun weapon;
    public Joystick joystick;

    [Space(10)]
    public EntityState state;

    [HideInInspector] public Vector3 moving;
    [HideInInspector] public Vector2 moveVelocity;
    [HideInInspector] public Vector3 dashDirection;
    [HideInInspector] public float zoomAmount;

    private NewInputSystem controls;

    //private Vector2 mousePos;

    private bool attack;

#if UNITY_ANDROID || UNITY_IOS
    private float lastMultiTouchDistance;
#endif

    private void Awake()
    {
        Instance = this;

        controls = new NewInputSystem();

        StaticGameVariables.InitializeLanguage();

        StaticGameVariables.InitializeAwake();

        state = EntityState.Normal;

        zoomAmount = PlayerPrefs.GetFloat("ZoomAmount", 0.6f);
    }

    private async void Start()
    {
        inventory = new Inventory();
        inventoryUI.SetInventory(inventory);

        controls.Player.Movement.performed += Movement_performed;
        controls.Player.Movement.canceled += Movement_canceled;

        controls.Player.Attack.performed += Attack_performed;
        controls.Player.Attack.canceled += Attack_canceled;

        controls.Player.Zoom.performed += Zoom_performed;

        /* Test */
        thisPlayer.TakeDamagePercent(0.5f, -1, null);

        GameObject test = await Pool.Instance.GetFromPoolAsync(0);
    }

    private void FixedUpdate()
    {
        if (state == EntityState.Normal)
        {
            if (moveVelocity != Vector2.zero)
            {
                rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
            }
        }
    }

    private void Update()
    {
        switch (state)
        {
            case EntityState.Normal:
                StateNormal();
                break;
            case EntityState.Dash:
                StateDash();
                break;
            case EntityState.Stun:
                StateStun();
                break;
            default:
                break;
        }
#if UNITY_ANDROID || UNITY_IOS
        if (!StaticGameVariables.isPause)
        {
            if (Touch.activeFingers.Count == 2 && joystick.Direction == Vector2.zero)
            {
                ZoomCamera(Touch.activeTouches[0], Touch.activeTouches[1]);
            }
        }
#endif
    }

#if UNITY_ANDROID || UNITY_IOS
    private void ZoomCamera(Touch firstTouch, Touch secondTouch)
    {
        if (firstTouch.phase == TouchPhase.Began || secondTouch.phase == TouchPhase.Began)
        {
            lastMultiTouchDistance = Vector2.Distance(firstTouch.screenPosition, secondTouch.screenPosition);
        }

        if (firstTouch.phase != TouchPhase.Moved || secondTouch.phase != TouchPhase.Moved)
        {
            return;
        }

        float newMultiTouchDistance = Vector2.Distance(firstTouch.screenPosition, secondTouch.screenPosition);
        Zoom(newMultiTouchDistance < lastMultiTouchDistance);
        lastMultiTouchDistance = newMultiTouchDistance;
    }
#endif

    private void StateNormal()
    {
        //mousePos = Camera.main.ScreenToWorldPoint(Pointer.current.position.ReadValue());

        if (moving.x == 0 && moving.y == 0)
        {
            moveVelocity = joystick.Direction.normalized * speed;
        }
        else
        {
            moveVelocity = moving.normalized * speed;
        }

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

        if (moveVelocity.x > 0f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (moveVelocity.x < 0f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    private void StateDash()
    {
        if (dash.nextDash <= Time.time)
        {
            rb.velocity = Vector2.zero;
            state = EntityState.Normal;
        }
        else
        {
            float dashSpeed = dash.dashSpeed.Evaluate(dash.dashEvaluateTime);
            rb.velocity = dashDirection * dashSpeed;
            dash.dashEvaluateTime += Time.deltaTime;
        }
    }

    private void StateStun()
    {
        return;
    }

    void Zoom(bool zoomOut)
    {
        cam.m_Lens.OrthographicSize = Mathf.Clamp(cam.m_Lens.OrthographicSize + (zoomOut ? zoomAmount : -zoomAmount), camMinSize, camMaxSize);
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
            state = EntityState.Dash;
        }
    }

    private void OnReload()
    {
        if (weapon && !weapon.reloading)
        {
            weapon.Reload();
        }
    }

    private void OnInventory()
    {
        if (!StaticGameVariables.isPause && !StaticGameVariables.inventoryCanvas.isActiveAndEnabled)
        {
            StaticGameVariables.OpenInventory();
        }
    }

    private void Attack_performed(InputAction.CallbackContext obj)
    {
        attack = true;
    }

    private void Attack_canceled(InputAction.CallbackContext obj)
    {
        attack = false;
    }

    private void Movement_performed(InputAction.CallbackContext obj)
    {
        moving = obj.ReadValue<Vector2>();
    }

    private void Movement_canceled(InputAction.CallbackContext obj)
    {
        moving = Vector3.zero;
    }

    private void Zoom_performed(InputAction.CallbackContext obj)
    {
        if (!StaticGameVariables.isPause)
        {
            Zoom(obj.ReadValue<Vector2>().y < 0f);
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
#if UNITY_ANDROID || UNITY_IOS
        EnhancedTouchSupport.Enable();
#endif
    }

    private void OnDisable()
    {
        controls.Disable();
#if UNITY_ANDROID || UNITY_IOS
        EnhancedTouchSupport.Disable();
#endif
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if ((weapon as NoWeapon).attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere((weapon as NoWeapon).attackPoint.position, weapon.gunData.radius);
    }
#endif
}
