using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using Pathfinding;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using Path = System.IO.Path;
#if UNITY_ANDROID || UNITY_IOS
using UnityEngine.EventSystems;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using UnityEngine.InputSystem.EnhancedTouch;
#endif

public class Player : AIEntity
{
    public static Player Instance { get; private set; }
    
    [Space(10)]
    public bool touch;
    //public bool weaponChangeTouch;

    [Space(10)]
    public Inventory inventory;
    public InventoryUI inventoryUI;
    public CinemachineVirtualCamera cam;
    public Camera mainCamera;
    public Stats stats;
    //public Transform gunPosition;

    [Space(10)]
    public bool gender = true;
    public int fightCount;
    
    public ItemWeapon weaponItem;
    
    /*public EquipableItem head;
    public EquipableItem torso;
    public EquipableItem legs;
    public EquipableItem foots;

    public ItemWeapon weaponItem;
    public ItemWeapon weaponRangedItem;
    public Gun weaponRanged;*/

    public Joystick joystick;
    
    [HideInInspector] public Vector2 moveVelocity;
    [HideInInspector] public Vector3 moving;
    [HideInInspector] public float zoomAmount;

    private Collider2D[] searchItem = new Collider2D[1];
    private const float searchItemRadius = 2f;
    private NewInputSystem controls;
    private LayerMask layerItems;

#if UNITY_ANDROID || UNITY_IOS
    private float lastMultiTouchDistance;
#endif

    private void Awake()
    {
        Instance = this;
        controls = new NewInputSystem();
        
        state = EntityState.Normal;
        thisEntity.OnHealthChanged -= OnDamaged;

        layerItems = 1 << 30;

        zoomAmount = PlayerPrefs.GetFloat("ZoomAmount", 0.6f);

        DontDestroyOnLoad(gameObject);
    }

    public void Initialize()
    {
        inventoryUI = GameObject.Find("ListSlots").GetComponent<InventoryUI>();
        inventoryUI.SetInventory(inventory);
        inventoryUI.Initialize();

        stats.Initialize();
        
        /*controls.Player.Touch.canceled += Touch_performed;
        controls.Player.WeaponChange.performed += WeaponChange_performed;
        controls.Player.WeaponChange.canceled += WeaponChange_canceled;*/
        controls.Player.Zoom.performed += Zoom_performed;
    }

    private void FixedUpdate()
    {
        if (state == EntityState.Normal && moveVelocity != Vector2.zero)
        {
            rb.MovePosition(rb.position + moveVelocity * (speed * Time.fixedDeltaTime));
        }
    }

    private void Update()
    {
        if (StaticGameVariables.isPause)
        {
            return;
        }

#if UNITY_ANDROID || UNITY_IOS
        if (state == EntityState.Attack)
        {
            StatePerform();
            return;
        }

        if (Touch.activeFingers.Count == 0)
        {
            joystick.gameObject.SetActive(false);
        }
        else if (Touch.activeFingers.Count == 1 && !joystick.gameObject.activeInHierarchy)
        {
            joystick.transform.position = Touch.activeTouches[0].screenPosition - (Vector2)joystick.transform.lossyScale / 2f;
            joystick.gameObject.SetActive(true);
            
            PointerEventData eventData = new PointerEventData(EventSystem.current)
            {
                position = Touch.activeTouches[0].screenPosition
            };
            
            joystick.OnDrag(eventData);
        }
        else if (Touch.activeFingers.Count == 2)
        {
            joystick.gameObject.SetActive(false);
            touch = false;
            ZoomCamera(Touch.activeTouches[0], Touch.activeTouches[1]);
            return;
        }
#endif

        StatePerform();
    }
    
    public override void StateNormal()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (joystick.Direction == Vector2.zero)
        {
            moveVelocity = Vector2.zero;
        }
        else
        {
            moveVelocity = joystick.Direction.normalized;
            
            if (moveVelocity.x > 0f)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else if (moveVelocity.x < 0f)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            
            targetDirection = moveVelocity;
        }
#endif
    }
    
    public override void StateDash()
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
    
    public override void StateStun()
    {
        moveVelocity = Vector2.zero;
    }
    
    public override void SetAnimation()
    {
        animations.SetInteger(StaticGameVariables.animationKeyID, (int)state);
        animations.SetBool(StaticGameVariables.animationMoveKeyID, moveVelocity != Vector2.zero);
    }
    
    public override void Attack()
    {
        if (!weapon)
        {
            state = EntityState.Normal;
            return;
        }
        
        if (weapon.nextAttack > Time.time)
        {
            return;
        }

        if (weapon.clip == 0)
        {
            weapon.fireWhenEmpty = true;
        }

        weapon.enabled = true;
        weapon.PrimaryAttack();
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

    public void PickUpItem(ItemWorld itemForPickup)
    {
        if (!itemForPickup)
        {
            return;
        }
        
        inventory.AddItem(itemForPickup.item);
        Addressables.ReleaseInstance(itemForPickup.gameObject);

        int size = Physics2D.OverlapCircleNonAlloc(transform.position, searchItemRadius, searchItem, layerItems);
        
        if (size > 0)
        {
            if (searchItem[0].TryGetComponent(out ItemWorld itemWorld))
            {
                PickUpItem(itemWorld);
            }
        }
    }

    private void Zoom(bool zoomOut)
    {
        cam.m_Lens.OrthographicSize = Mathf.Clamp(cam.m_Lens.OrthographicSize + (zoomOut ? zoomAmount : -zoomAmount),
            StaticGameVariables.camMinSize, StaticGameVariables.camMaxSize);
    }

    private void OnDash()
    {
        if (!StaticGameVariables.isPause && state == EntityState.Normal && stats.stamina > dash.staminaCost && dash.nextDashTime <= Time.time)
        {
            //weaponChangeTouch = false;
            stats.stamina -= dash.staminaCost;
            dash.dashEvaluateTime = 0f;
            dashDirection = moveVelocity;
            dash.enabled = true;
            dash.Use();
            state = EntityState.Dash;
        }
    }

    private void OnReload()
    {
        if (StaticGameVariables.isPause || !weapon || weapon.reloading || state != EntityState.Normal)
        {
            return;
        }
        
        //weaponChangeTouch = false;
        weapon.Reload();
    }

    private void OnInventory()
    {
        if (StaticGameVariables.isPause && StaticGameVariables.inventoryCanvas.isActiveAndEnabled)
        {
            return;
        }
        
        StaticGameVariables.OpenInventory();
    }

    public void OnLoad()
    {
        if (StaticGameVariables.isPause)
        {
            return;
        }
        
        StringBuilder sb = new StringBuilder(Path.Combine(StaticGameVariables._SAVE_FOLDER, "save0.json"));

        if (File.Exists(sb.ToString()))
        {
            Settings.Instance.gameIsLoaded = true;
        }
        
        SceneLoading.Instance.LoadLevel("World");
    }
    
    public override void OnPause(object sender, EventArgs e)
    {
        moveVelocity = Vector2.zero;
        animations.speed = StaticGameVariables.isPause ? 0f : 1f;
    }

    /*private void Touch_performed(InputAction.CallbackContext obj)
    {
        if (StaticGameVariables.isPause)
        {
            return;
        }
        
        //touch = true;
    }
    
    private void WeaponChange_performed(InputAction.CallbackContext obj)
    {
        if (StaticGameVariables.isPause)
        {
            return;
        }
        
        //weaponChangeTouch = true;
    }
    
    private void WeaponChange_canceled(InputAction.CallbackContext obj)
    {
        if (StaticGameVariables.isPause)
        {
            return;
        }
        
        //weaponChangeTouch = false;
    }*/

    private void Zoom_performed(InputAction.CallbackContext obj)
    {
        if (StaticGameVariables.isPause)
        {
            return;
        }
        
#if UNITY_STANDALONE_LINUX || UNITY_EDITOR_LINUX
        Zoom(obj.ReadValue<Vector2>().y > 0f);
#else
        Zoom(obj.ReadValue<Vector2>().y < 0f);
#endif
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)GameLayers.Spawners) // Spawners
        {
            if (collision.TryGetComponent(out EntityMaker entityMaker))
            {
                entityMaker.isTrigger = true;
                entityMaker.enabled = true;
            }
            
            return;
        }
        
        if (collision.gameObject.layer == (int)GameLayers.Items) // Items
        {
            if (collision.TryGetComponent(out ItemWorld itemWorld))
            {
                PickUpItem(itemWorld);
            }
        }
    }
    
    public override void EventEnable()
    {
        base.EventEnable();
        controls.Enable();
#if UNITY_ANDROID || UNITY_IOS
        EnhancedTouchSupport.Enable();
#endif
    }

    public override void EventDisable()
    {
        base.EventDisable();
        controls.Disable();
#if UNITY_ANDROID || UNITY_IOS
        EnhancedTouchSupport.Disable();
#endif
    }
    
    public override void EventDestroy()
    {
        base.EventDestroy();
        controls.Disable();
#if UNITY_ANDROID || UNITY_IOS
        EnhancedTouchSupport.Disable();
#endif
    }
}
