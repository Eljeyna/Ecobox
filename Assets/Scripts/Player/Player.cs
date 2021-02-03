using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using Pathfinding;

#if UNITY_ANDROID || UNITY_IOS
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using UnityEngine.InputSystem.EnhancedTouch;
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

    public float speed = 4f;
    public float speedSlow;
    public bool touch = false;
    public bool buttonTouch = false;

    [Space(10)]
    public Inventory inventory;
    public InventoryUI inventoryUI;
    public Dash dash;
    public Animator animations;
    public Rigidbody2D rb;
    public CinemachineVirtualCamera cam;
    public Camera mainCamera;
    public BasePlayer thisPlayer;
    public Stats stats;
    public Gun weapon;
    public AIDestinationSetter aiEntity;
    public AIPath aiPath;
    public Transform target;
    public BuffSystem buffSystem;

    [Space(10)]
    public EntityState state;

    [HideInInspector] public Vector3 moving;
    [HideInInspector] public Vector2 moveVelocity;
    [HideInInspector] public Vector3 dashDirection;
    [HideInInspector] public float zoomAmount;

    private NewInputSystem controls;

    private Collider2D[] entity = new Collider2D[1];
    private LayerMask layer;
    private float defaultEndReachedDistance;

#if UNITY_ANDROID || UNITY_IOS
    private float lastMultiTouchDistance;
#endif

    private void Awake()
    {
        Instance = this;
        controls = new NewInputSystem();

        defaultEndReachedDistance = aiPath.endReachedDistance;

        state = EntityState.Normal;
        layer = 1 << gameObject.layer;

        zoomAmount = PlayerPrefs.GetFloat("ZoomAmount", 0.6f);

        DontDestroyOnLoad(gameObject);
    }

    public async void Initialize()
    {
        inventoryUI = GameObject.Find("ListSlots").GetComponent<InventoryUI>();
        inventoryUI.SetInventory(inventory);

        stats.Initialize();

#if (!UNITY_ANDROID && !UNITY_IOS) || UNITY_EDITOR
        controls.Player.Touch.performed += Touch_performed;
#endif

        controls.Player.Movement.performed += Movement_performed;
        controls.Player.Movement.canceled += Movement_canceled;

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
#if UNITY_ANDROID || UNITY_IOS || UNITY_EDITOR
        if (!StaticGameVariables.isPause && !buttonTouch)
        {
            if (Touch.activeFingers.Count == 1 && Touch.activeTouches[0].phase == TouchPhase.Began)
            {
                touch = true;
            }
            else if (Touch.activeFingers.Count == 2)
            {
                touch = false;
                ZoomCamera(Touch.activeTouches[0], Touch.activeTouches[1]);
            }
        }
#endif

        if (touch && !buttonTouch)
        {
            touch = false;
            target.position = mainCamera.ScreenToWorldPoint(Pointer.current.position.ReadValue());

            int length = Physics2D.OverlapCircleNonAlloc(target.position, aiPath.radius, entity, layer);
            if (length > 0)
            {
                aiPath.endReachedDistance = GetEndReachedDistance();
            }
            else
            {
                entity = null;
                aiEntity.target = target;
                aiPath.endReachedDistance = defaultEndReachedDistance;
            }
        }
        
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
        if (!aiEntity.isActiveAndEnabled)
        {
            aiEntity.enabled = true;
        }

        if (aiEntity.target == null)
        {
            return;
        }

        float distance = Vector2.Distance(rb.position, aiEntity.target.position);
        
        if (distance <= aiPath.endReachedDistance)
        {
            if (entity != null)
            {
                Attack();
            }
            else
            {
                aiEntity.target = null;
                return;
            }
        }

        float angle = StaticGameVariables.GetAngleBetweenPositions(aiEntity.target.position, transform.position);

        if (angle <= 90f && angle >= -90f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    private void StateDash()
    {
        if (dash.nextDash <= Time.time)
        {
            aiEntity.enabled = true;
            aiPath.enabled = true;
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
        if (aiEntity.isActiveAndEnabled)
        {
            aiEntity.enabled = false;
        }

        return;
    }
    
    public void Attack()
    {
        if (weapon.nextAttack <= Time.time)
        {
            if (weapon.clip == 0)
            {
                weapon.fireWhenEmpty = true;
            }

            weapon.PrimaryAttack();
        }
    }

    private void Zoom(bool zoomOut)
    {
        cam.m_Lens.OrthographicSize = Mathf.Clamp(cam.m_Lens.OrthographicSize + (zoomOut ? zoomAmount : -zoomAmount), StaticGameVariables.camMinSize, StaticGameVariables.camMaxSize);
    }

    private void OnDash()
    {
        if (aiEntity.target != null && stats.stamina > dash.staminaCost && dash.nextDashTime <= Time.time)
        {
            aiEntity.enabled = false;
            aiPath.enabled = false;
            stats.stamina -= dash.staminaCost;
            dash.dashEvaluateTime = 0f;
            //dashDirection = moving == Vector3.zero ? transform.up : moving.normalized;
            moving = target.position - transform.position;
            dashDirection = moving.normalized;
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

    private void OnSave()
    {
        SaveLoadSystem.Instance.Save();
    }

    private void OnLoad()
    {
        SaveLoadSystem.Instance.Load();
    }

#if (!UNITY_ANDROID && !UNITY_IOS) || UNITY_EDITOR
    private void Touch_performed(InputAction.CallbackContext obj)
    {
        if (!StaticGameVariables.isPause)
        {
            touch = true;
        }
    }
#endif

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

    private float GetEndReachedDistance()
    {
        aiEntity.target = entity[0].transform;
        CapsuleCollider2D collider = entity[0].GetComponent<CapsuleCollider2D>();
        if (collider != null)
        {
            return weapon.gunData.range + StaticGameVariables.GetReachedDistance(collider);
        }
        else
        {
            return weapon.gunData.range;
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
