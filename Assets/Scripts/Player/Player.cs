using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Cinemachine;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
#if UNITY_ANDROID || UNITY_IOS
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using UnityEngine.InputSystem.EnhancedTouch;
#endif

public class Player : AIEntity, ISaveState
{
    public static Player Instance { get; private set; }
    
    [Space(10)]
    public bool touch;

    [Space(10)]
    public Dash dash;
    public Inventory inventory;
    public InventoryUI inventoryUI;
    public CinemachineVirtualCamera cam;
    public Stats stats;
    public Transform gunPosition;

    [Space(10)]
    public bool gender; // Female
    public bool dashing = true;
    public int fightCount;
    
    public ItemWeapon weaponItem;
    
    /*
    public EquipableItem head;
    public EquipableItem torso;
    public EquipableItem legs;
    public EquipableItem foots;
    */

    public Joystick joystickMove;
    public Joystick joystickAttack;
    
    //[HideInInspector] public Vector2 moveVelocity;
    [HideInInspector] public float zoomAmount;

    private Collider2D[] searchItem = new Collider2D[1];
    //private const float searchEntityRadius = 3f;
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
        entity = new Collider2D[2];

        zoomAmount = PlayerPrefs.GetFloat("ZoomAmount", 0.6f);
    }

    public void Initialize()
    {
        inventoryUI = GameObject.Find("ListSlots").GetComponent<InventoryUI>();
        inventoryUI.SetInventory(inventory);
        inventoryUI.Initialize();

        stats.Initialize();

        controls.Player.Attack.performed += Attack_performed;

        controls.Player.Zoom.performed += Zoom_performed;
    }

    private void FixedUpdate()
    {
        if (StaticGameVariables.isPause)
        {
            return;
        }

        if (isJumping)
        {
            isJumping = false;
            rb.AddForce(Vector2.up * StaticGameVariables.globalJumpForce, ForceMode2D.Impulse);
        }

        isGrounded = IsGrounded();

        if (isGrounded)
        {
            animations.SetFloat(StaticGameVariables.animationFallKeyID, 0f);

            if (!dashing)
            {
                dashing = true;
            }
        }
        else if (!isGrounded)
        {
            float fallTime = animations.GetFloat(StaticGameVariables.animationFallKeyID);

            if (fallTime < 1f)
            {
                animations.SetFloat(StaticGameVariables.animationFallKeyID, fallTime + Time.fixedDeltaTime);
            }
        }
    }

    private void Update()
    {
        if (StaticGameVariables.isPause)
        {
            return;
        }

#if UNITY_ANDROID || UNITY_IOS
        if (!touch || state == EntityState.Attack)
        {
            StatePerform();
            return;
        }

        if (Touch.activeFingers.Count == 2)
        {
            touch = false;
            ZoomCamera(Touch.activeTouches[0], Touch.activeTouches[1]);
            return;
        }
#endif

        StatePerform();
    }
    
    public override void StateNormal()
    {
        float moveX = controls.Player.Move.ReadValue<float>();

        if (moveX == 0f)
        {
            moveVelocity = 0f;
            rb.velocity = new Vector2(moveVelocity, rb.velocity.y);
        }
        else
        {
            moveVelocity = moveX * Speed;
            rb.velocity = new Vector2(moveVelocity, rb.velocity.y);
            targetDirection = new Vector3(moveX, 0f, 0f);

            if (moveVelocity > 0f)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else if (moveVelocity < 0f)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            return;
        }

#if UNITY_ANDROID || UNITY_IOS
        if (joystickMove.Direction == Vector2.zero)
        {
            moveVelocity = 0f;
            rb.velocity = new Vector2(0f, rb.velocity.y);
        }
        else
        {
            moveVelocity = joystickMove.Direction.normalized.x;
            targetDirection = new Vector3(moveX, 0f, 0f);

            if (joystickAttack.Direction.x == 0f)
            {
                rb.velocity = new Vector2(moveVelocity, rb.velocity.y);

                if (moveVelocity > 0f)
                {
                    transform.localScale = new Vector3(1f, 1f, 1f);
                }
                else if (moveVelocity < 0f)
                {
                    transform.localScale = new Vector3(-1f, 1f, 1f);
                }
            }
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
        rb.velocity = new Vector2(0f, rb.velocity.y);
    }
    
    public override void StateDeath()
    {
        rb.velocity = new Vector2(0f, rb.velocity.y);
        state = EntityState.None;
        OnLoad();
    }
    
    public override void SetAnimation()
    {
        animations.SetInteger(StaticGameVariables.animationKeyID, (int)state);
        animations.SetBool(StaticGameVariables.animationMoveKeyID, moveVelocity != 0f);
        animations.SetBool(StaticGameVariables.animationJumpKeyID, !isGrounded);
    }
    
    public override void Attack()
    {
        if (!gameObject)
        {
            return;
        }

#if UNITY_ANDROID || UNITY_IOS
        if (joystickAttack.Direction.x > 0f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (joystickAttack.Direction.x < 0f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
#endif

        if (!weapon)
        {
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
        
        /*if (!target)
        {
            Physics2D.OverlapCircleNonAlloc(transform.position, 
                weapon.gunData.range + weapon.gunData.radius + searchEntityRadius,
                entity, 1 << gameObject.layer);

            if (entity[1])
            {
                if (entity[1].TryGetComponent(out BaseTag tagEntity) && Damage.IsEnemy(tagEntity, thisTag))
                {
                    target = entity[1].transform;
                    targetDirection = (target.position - transform.position).normalized;
                }  
            }
        }

        if (target)
        {
            float angle = StaticGameVariables.GetAngleBetweenPositions(target.position, transform.position);

            if (angle <= 90f && angle >= -90f)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }
            else
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
        }*/

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
        itemForPickup.gameObject.SetActive(false);
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

    public void OnDash()
    {
        if (!StaticGameVariables.isPause && dashing && state == EntityState.Normal && stats.stamina > dash.staminaCost && dash.nextDashTime <= Time.time)
        {
            dashing = false;
            stats.nextStaminaRegen = Time.time + stats.staminaTimeRegenWhenUse;
            stats.stamina -= dash.staminaCost;
            stats.OnStaminaChanged?.Invoke(this, EventArgs.Empty);
            dash.dashEvaluateTime = 0f;
            dashDirection = targetDirection;
            dash.enabled = true;
            dash.Use();
            state = EntityState.Dash;
        }
    }

    public void OnJump()
    {
        if (StaticGameVariables.isPause || state != EntityState.Normal || !IsGrounded())
        {
            return;
        }

        isJumping = true;
    }

    public void OnReload()
    {
        if (StaticGameVariables.isPause || state != EntityState.Normal || !weapon || weapon.reloading)
        {
            return;
        }
        
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
        //StringBuilder sb = new StringBuilder(Path.Combine(StaticGameVariables._SAVE_FOLDER, "save0.json"));
        SceneLoading.Instance.SwitchToScene(SceneManager.GetActiveScene().name, SceneLoading.startAnimationID);
    }
    
    public override void OnPause(object sender, EventArgs e)
    {
        rb.simulated = !StaticGameVariables.isPause;
        animations.speed = StaticGameVariables.isPause ? 0f : 1f;
    }


    private void Attack_performed(InputAction.CallbackContext obj)
    {
        if (StaticGameVariables.isPause || state != EntityState.Normal)
        {
            return;
        }

        touch = false;
        Attack();
    }


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
    
    public string Save()
    {
        Saveable saveObject = new Saveable();
        int i = 0;
        
        if (inventory.itemList.Count > 0)
        {
            saveObject.itemsID = new string[inventory.itemList.Count];
            saveObject.itemsAmount = new int[inventory.itemList.Count];
            foreach (string key in inventory.itemList.Keys)
            {
                saveObject.itemsID[i] = inventory.itemList[key].idReference.AssetGUID;
                saveObject.itemsAmount[i] = inventory.itemList[key].itemAmount;
                i++;
            }
        }
        
        if (GameDirector.Instance.quests.Count > 0)
        {
            i = 0;
            
            saveObject.questID = new string[GameDirector.Instance.quests.Count];
            saveObject.questTask = new int[GameDirector.Instance.quests.Count];
            foreach (string key in GameDirector.Instance.quests.Keys)
            {
                saveObject.questID[i] = GameDirector.Instance.quests[key].id;
                saveObject.questTask[i] = GameDirector.Instance.quests[key].currentTask;
                i++;
            }
        }
        
        ScriptableObjectBuff[] buffsList = new ScriptableObjectBuff[buffSystem.buffs.Count];
        if (buffSystem.buffs.Count > 0)
        {
            i = 0;
            
            saveObject.buffsID = new string[buffSystem.buffs.Count];
            saveObject.buffsDuration = new float[buffSystem.buffs.Count];
            saveObject.buffsStacks = new int[buffSystem.buffs.Count];
            
            foreach (ScriptableObjectBuff key in buffSystem.buffs.Keys)
            {
                saveObject.buffsID[i] = key.idReference.AssetGUID;
                saveObject.buffsDuration[i] = key.duration;
                saveObject.buffsStacks[i] = key.buff.stacks;
                buffsList[i] = key;
                i++;
            }

            foreach (ScriptableObjectBuff key in buffsList)
            {
                buffSystem.RemoveBuff(key.buff);
            }
        }
        
        StringBuilder saveBuilder = new StringBuilder(Path.Combine(StaticGameVariables._SAVE_FOLDER, "save0.json")); 
        StringBuilder sb = new StringBuilder(Path.Combine(StaticGameVariables._SAVE_FOLDER, "cplQ.json")); // file with Completed Quests (temporary)

        if (File.Exists(saveBuilder.ToString()))
        {
            saveObject.completedQuestsID = JsonConvert.DeserializeObject<CompletedQuestsID>(File.ReadAllText(saveBuilder.ToString())).completedQuestsID;
            if (File.Exists(sb.ToString()))
            {
                CompletedQuestsID jsonQuests = JsonConvert.DeserializeObject<CompletedQuestsID>(File.ReadAllText(sb.ToString()));

                if (jsonQuests.completedQuestsID.Count > 0)
                {
                    foreach (string key in jsonQuests.completedQuestsID.Keys)
                    {
                        if (!saveObject.completedQuestsID.TryGetValue(key, out int value))
                        {
                            saveObject.completedQuestsID.Add(key, value);
                        }
                    }

                    CompletedQuestsID zeroing = new CompletedQuestsID {completedQuestsID = new Dictionary<string, int>()};
                    File.WriteAllText(sb.ToString(), JsonConvert.SerializeObject(zeroing));
                }
            }
        }
        else if (File.Exists(sb.ToString()))
        {
            CompletedQuestsID jsonQuests = JsonConvert.DeserializeObject<CompletedQuestsID>(File.ReadAllText(sb.ToString()));

            if (jsonQuests.completedQuestsID.Count > 0)
            {
                saveObject.completedQuestsID = new Dictionary<string, int>();
                foreach (string key in jsonQuests.completedQuestsID.Keys)
                {
                    saveObject.completedQuestsID.Add(key, 0);
                }

                CompletedQuestsID zeroing = new CompletedQuestsID {completedQuestsID = new Dictionary<string, int>()};
                File.WriteAllText(sb.ToString(), JsonConvert.SerializeObject(zeroing));
            }
        }
        
        if (GameDirector.Instance.activeQuest != null)
        {
            saveObject.activeQuestID = GameDirector.Instance.activeQuest.id;
        }
        else
        {
            saveObject.activeQuestID = string.Empty;
        }

        /*
        if (head)
        {
            saveObject.head = head.itemName;
        }
        else
        {
            saveObject.head = string.Empty;
        }
        
        if (torso)
        {
            saveObject.torso = torso.itemName;
        }
        else
        {
            saveObject.torso = string.Empty;
        }
        
        if (legs)
        {
            saveObject.legs = legs.itemName;
        }
        else
        {
            saveObject.legs = string.Empty;
        }
        
        if (foots)
        {
            saveObject.foots = foots.itemName;
        }
        else
        {
            saveObject.foots = string.Empty;
        }
        if (weaponRangedItem)
        {
            saveObject.weaponRanged = weaponRangedItem.itemName;
        }
        else
        {
            saveObject.weaponRanged = string.Empty;
        }
        */
        
        if (weaponItem)
        {
            saveObject.weapon = weaponItem.itemName;
        }
        else
        {
            saveObject.weapon = string.Empty;
        }

        saveObject.maxStamina = stats.maxStamina;
        saveObject.stamina = stats.stamina;
        saveObject.staminaRegen = stats.staminaRegen;
        saveObject.staminaTimeRegen = stats.staminaTimeRegen;
        /*
        saveObject.level = level;
        saveObject.exp = exp;
        saveObject.talentPoints = talentPoints;
        saveObject.weight = weight;
        saveObject.strength = strength;
        saveObject.agility = agility;
        saveObject.intelligence = intelligence;
        saveObject.oratory = oratory;
        */
        saveObject.money = stats.money;

        saveObject.maxHealth = thisEntity.maxHealth;
        saveObject.health = thisEntity.health;
        saveObject.healthPercent = thisEntity.healthPercent;
        saveObject.resistances = thisEntity.resistances;
        saveObject.invinsibility =thisEntity.invinsibility;
        /*
        saveObject.positionX = transform.position.x;
        saveObject.positionY = transform.position.y;
        */

        if (buffsList.Length > 0)
        {
            i = 0;

            foreach (ScriptableObjectBuff key in buffsList)
            {
                buffSystem.AddBuff(key.InitializeBuff(gameObject));
                buffSystem.buffs[key].duration = saveObject.buffsDuration[i];
                buffSystem.buffs[key].stacks = saveObject.buffsStacks[i];
                i++;
            }
        }
        
        return JsonConvert.SerializeObject(saveObject);
    }

    public async Task Load()
    {
        StringBuilder sb = new StringBuilder(Path.Combine(StaticGameVariables._SAVE_FOLDER, "save0.json"));

        if (File.Exists(sb.ToString()))
        {
            Saveable saveObject = JsonConvert.DeserializeObject<Saveable>(File.ReadAllText(sb.ToString()));

            if (!ReferenceEquals(saveObject.itemsID, null) && saveObject.itemsID.Length > 0)
            {
                inventory.ClearInventory();
                for (int i = 0; i < saveObject.itemsID.Length; i++)
                {
                    Item item = await Database.GetItem<Item>(saveObject.itemsID[i]);
                    inventory.AddItem(item, saveObject.itemsAmount[i]);
                    Addressables.Release(item);
                }
            }

            if (!ReferenceEquals(saveObject.questID, null) && saveObject.questID.Length > 0)
            {
                GameDirector.Instance.quests.Clear();
                for (int i = 0; i < saveObject.questID.Length; i++)
                {
                    GameDirector.Instance.quests.Add(saveObject.questID[i], new Quest(saveObject.questID[i], saveObject.questTask[i]));
                }
            }

            stats.maxStamina = saveObject.maxStamina;
            stats.stamina = saveObject.stamina;
            stats.staminaRegen = saveObject.staminaRegen;
            stats.staminaTimeRegen = saveObject.staminaTimeRegen;
            /*
            level = saveObject.level;
            exp = saveObject.exp;
            talentPoints = saveObject.talentPoints;
            weight = saveObject.weight;
            strength = saveObject.strength;
            agility = saveObject.agility;
            intelligence = saveObject.intelligence;
            oratory = saveObject.oratory;
            */
            stats.money = saveObject.money;

            //aiEntity.target = null;
            thisEntity.health = saveObject.health;
            thisEntity.healthPercent = saveObject.healthPercent;
            thisEntity.resistances = saveObject.resistances;
            thisEntity.invinsibility = saveObject.invinsibility;
            thisEntity.SetMaxHealth(saveObject.maxHealth);
            /*
            transform.position = new Vector3(saveObject.positionX, saveObject.positionY, 0f);
            aiEntity.target = transform;
            */

            if (saveObject.activeQuestID != string.Empty)
            {
                GameDirector.Instance.activeQuest = GameDirector.Instance.quests[saveObject.activeQuestID];
                GameDirector.Instance.UpdateQuestDescription();
            }
            
            /*
            if (saveObject.head != string.Empty)
            {
                head = (EquipableItem)inventory.itemList[saveObject.head];
            }
            
            if (saveObject.torso != string.Empty)
            {
                head = (EquipableItem)inventory.itemList[saveObject.torso];
            }
            
            if (saveObject.legs != string.Empty)
            {
                head = (EquipableItem)inventory.itemList[saveObject.legs];
            }
            
            if (saveObject.foots != string.Empty)
            {
                head = (EquipableItem)inventory.itemList[saveObject.foots];
            }
            */
            
            if (saveObject.weapon != string.Empty)
            {
                inventory.itemList[saveObject.weapon].Use();
            }

            /*
             if (saveObject.weaponRanged != string.Empty)
            {
                inventory.itemList[saveObject.weaponRanged].Use();
            }
            */
            
            if (!ReferenceEquals(saveObject.buffsID, null))
            {
                for (int i = 0; i < saveObject.buffsID.Length; i++)
                {
                    ScriptableObjectBuff buff = await Database.GetItem<ScriptableObjectBuff>(saveObject.buffsID[i]);
                    buffSystem.AddBuff(buff.InitializeBuff(gameObject));
                    buffSystem.buffs[buff].duration = saveObject.buffsDuration[i];
                    buffSystem.buffs[buff].stacks = saveObject.buffsStacks[i];
                    Addressables.Release(buff);
                }
            }
        }
    }
}
