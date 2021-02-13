using System.IO;
using System.Text;
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
    public bool touch;
    public bool buttonTouch;

    [Space(10)]
    public InventoryUI inventoryUI;
    public CinemachineVirtualCamera cam;
    public Camera mainCamera;
    public Stats stats;

    [Space(10)]
    public bool gender = true;
    
    [SerializeField] private AssetReferenceAtlasedSprite atlasSprite;

    [HideInInspector] public Vector3 moving;
    [HideInInspector] public float zoomAmount;

    private ItemWorld itemForPickup;
    private NewInputSystem controls;
    private LayerMask layer;

#if UNITY_ANDROID || UNITY_IOS
    private float lastMultiTouchDistance;
#endif

    private void Awake()
    {
        Instance = this;
        controls = new NewInputSystem();

        InitializeEntity();

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

        controls.Player.Touch.performed += Touch_performed;
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

        if (touch && !buttonTouch && state != EntityState.Attack)
        {
            touch = false;
            targetPosition = mainCamera.ScreenToWorldPoint(Pointer.current.position.ReadValue());
            targetPosition.z = 0f;

            if (!target)
            {
                isEnemy = false;
                return;
            }
            
            target.position = targetPosition;

            int length = Physics2D.OverlapCircleNonAlloc(target.position, aiPath.radius, entity, layer);
            if (length > 0 && entity[0] != thisCollider)
            {
                aiPath.endReachedDistance = GetEndReachedDistance() - defaultEndReachedDistance;
                
                if (entity[0].TryGetComponent(out BaseTag anotherEntityTag) && Damage.IsEnemy(thisTag, anotherEntityTag))
                {
                    isEnemy = true;
                }
                else
                {
                    isEnemy = false;
                }
            }
            else
            {
                isEnemy = false;
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

    public void PickUpItem()
    {
        if (ReferenceEquals(itemForPickup, null))
        {
            return;
        }
        
        inventory.AddItem(itemForPickup.item);
        Addressables.ReleaseInstance(itemForPickup.gameObject);
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
        if (!StaticGameVariables.isPause && !ReferenceEquals(aiEntity.target, null) && stats.stamina > dash.staminaCost && dash.nextDashTime <= Time.time)
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
        if (!StaticGameVariables.isPause &&weapon && !weapon.reloading)
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

    public void OnSave()
    {
        if (StaticGameVariables.isPause)
        {
            return;
        }
        
        SaveLoadSystem.Instance.Save();
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

    private void Touch_performed(InputAction.CallbackContext obj)
    {
        if (!StaticGameVariables.isPause)
        {
            touch = true;
        }
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
        if (collision.gameObject.layer == 20) // Spawners
        {
            if (collision.TryGetComponent(out EntityMaker entityMaker))
            {
                entityMaker.isTrigger = true;
                entityMaker.enabled = true;
            }
            
            return;
        }
        
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
        if (atlasSprite.IsValid())
        {
            atlasSprite.ReleaseAsset();
        }
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
