using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
#if UNITY_ANDROID || UNITY_IOS
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using UnityEngine.InputSystem.EnhancedTouch;
#endif

public class Player : AIEntity, ITranslate
{
    public static Player Instance { get; private set; }
    
    [Space(10)]
    public bool touch = false;
    public bool buttonTouch = false;

    [Space(10)]
    public Inventory inventory;
    public InventoryUI inventoryUI;
    public Dash dash;
    public Animator animations;
    public CinemachineVirtualCamera cam;
    public Camera mainCamera;
    public Stats stats;

    [Space(10)]
    public bool gender = true;
    
    [SerializeField] private AssetReferenceAtlasedSprite atlasSprite;

    [HideInInspector] public Vector3 moving;
    [HideInInspector] public Vector2 moveVelocity;
    [HideInInspector] public Vector3 dashDirection;
    [HideInInspector] public float zoomAmount;

    private ItemWorld itemForPickup;

    private NewInputSystem controls;

    private readonly Collider2D[] entity = new Collider2D[1];
    private LayerMask layer;

#if UNITY_ANDROID || UNITY_IOS
    private float lastMultiTouchDistance;
#endif

    private void Awake()
    {
        Instance = this;
        controls = new NewInputSystem();

        InitializeEntity();

        state = EntityState.Normal;
        layer = 1 << gameObject.layer;

        zoomAmount = PlayerPrefs.GetFloat("ZoomAmount", 0.6f);

        DontDestroyOnLoad(gameObject);
    }

    public async void Initialize()
    {
        inventoryUI = GameObject.Find("ListSlots").GetComponent<InventoryUI>();
        inventoryUI.SetInventory(inventory);
        inventoryUI.Initialize();

        stats.Initialize();

#if UNITY_ANDROID || UNITY_IOS
        controls.Player.Touch.performed += Touch_performed;
#endif

        controls.Player.Movement.performed += Movement_performed;
        controls.Player.Movement.canceled += Movement_canceled;

        controls.Player.Zoom.performed += Zoom_performed;

        GameObject targetNew = await Pool.Instance.GetFromPoolAsync((int)PoolID.Target);
        targetNew.AddComponent(typeof(SpriteRenderer));
        AsyncOperationHandle<Sprite> asyncOperationHandle = atlasSprite.LoadAssetAsync<Sprite>();
        await asyncOperationHandle.Task;
        if (targetNew.TryGetComponent(out SpriteRenderer newSpriteRenderer))
        {
            newSpriteRenderer.color = new Color(1f, 1f, 1f, 64f / 255f);
            
            if (asyncOperationHandle.Status == AsyncOperationStatus.Succeeded)
            {
                newSpriteRenderer.sprite = asyncOperationHandle.Result;
            }
        }
        
        target = targetNew.transform;
    }

    private void Update()
    {
#if UNITY_ANDROID || UNITY_IOS
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
            targetPosition = mainCamera.ScreenToWorldPoint(Pointer.current.position.ReadValue());
            targetPosition.z = 0f;
            target.position = targetPosition;

            int length = Physics2D.OverlapCircleNonAlloc(target.position, aiPath.radius, entity, layer);
            if (length > 0 && entity[0] != thisCollider)
            {
                aiPath.endReachedDistance = GetEndReachedDistance() - defaultEndReachedDistance;
            }
            else
            {
                aiEntity.target = target;
                aiPath.endReachedDistance = defaultEndReachedDistance;
            }
        }

        StatePerform();
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

    public override void StateNormal()
    {
        if (!aiPath.isActiveAndEnabled)
        {
            aiPath.enabled = true;
        }

        if (ReferenceEquals(aiEntity.target, null))
        {
            return;
        }

        float distance = Vector2.Distance(rb.position, aiEntity.target.position);
        
        if (distance <= aiPath.endReachedDistance)
        {
            if (ReferenceEquals(entity, null))
            {
                aiEntity.target = null;
                return;
            }
            else
            {
                Attack();
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

    public override void StateDash()
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

    public override void StateStun()
    {
        if (aiPath.isActiveAndEnabled)
        {
            aiPath.enabled = false;
        }
    }

    public override void StateAttack()
    {
        return;
    }
    
    public override void StateCast()
    {
        return;
    }

    public void PickUpItem()
    {
        if (ReferenceEquals(itemForPickup, null))
        {
            return;
        }
        
        inventory.AddItem(itemForPickup.item);
        Destroy(itemForPickup.gameObject);
    }
    
    public void GetTranslate()
    {
        if (ReferenceEquals(itemForPickup, null))
        {
            return;
        }
        
        if (StaticGameVariables.itemPickupCanvas.isActiveAndEnabled)
        {
            StaticGameVariables.UpdateItemPickableInfo(itemForPickup);
        }
    }

    private void Zoom(bool zoomOut)
    {
        cam.m_Lens.OrthographicSize = Mathf.Clamp(cam.m_Lens.OrthographicSize + (zoomOut ? zoomAmount : -zoomAmount), StaticGameVariables.camMinSize, StaticGameVariables.camMaxSize);
    }

    private void OnDash()
    {
        if (!ReferenceEquals(aiEntity.target, null) && stats.stamina > dash.staminaCost && dash.nextDashTime <= Time.time)
        {
            aiEntity.enabled = false;
            aiPath.enabled = false;
            stats.stamina -= dash.staminaCost;
            dash.dashEvaluateTime = 0f;
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

#if UNITY_ANDROID || UNITY_IOS
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
#if UNITY_STANDALONE_LINUX || UNITY_EDITOR_LINUX
            Zoom(obj.ReadValue<Vector2>().y > 0f);
#else
            Zoom(obj.ReadValue<Vector2>().y < 0f);
#endif
        }
    }

    private new float GetEndReachedDistance()
    {
        aiEntity.target = entity[0].transform;
        return base.GetEndReachedDistance();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 30) // Items
        {
            if (collision.TryGetComponent(out ItemWorld itemWorld))
            {
                itemForPickup = itemWorld;
                
                if (StaticGameVariables.itemPickupCanvas.isActiveAndEnabled)
                {
                    StaticGameVariables.UpdateItemPickableInfo(itemWorld);
                }
                else
                {
                    StaticGameVariables.ShowItemPickableInfo(itemWorld);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 30) // Items
        {
            if (collision.TryGetComponent(out ItemWorld itemWorld) && ReferenceEquals(itemForPickup, itemWorld))
            {
                itemForPickup = null;
                StaticGameVariables.HideItemPickableInfo();
            }
        }
    }

    private void OnDestroy()
    {
        if (atlasSprite.IsValid())
        {
            atlasSprite.ReleaseAsset();
        }

        aiEntity.target = null;
    }

    private void OnEnable()
    {
        StaticGameVariables.OnPauseGame += OnPause;
        controls.Enable();
#if UNITY_ANDROID || UNITY_IOS
        EnhancedTouchSupport.Enable();
#endif
    }

    private void OnDisable()
    {
        StaticGameVariables.OnPauseGame -= OnPause;
        controls.Disable();
#if UNITY_ANDROID || UNITY_IOS
        EnhancedTouchSupport.Disable();
#endif
    }

#if UNITY_EDITOR
    /*private void OnDrawGizmos()
    {
        if ((weapon as NoWeapon).attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere((weapon as NoWeapon).attackPoint.position, weapon.gunData.radius);
    }*/
#endif
}
