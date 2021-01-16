using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    [HideInInspector] public Inventory inventory;

    [SerializeField] private InventoryUI inventoryUI;

    private NewInputSystem controls;

    private void Awake()
    {
        controls = new NewInputSystem();
    }

    private void Start()
    {
        Instance = this;

        inventory = new Inventory();
        inventoryUI.SetInventory(inventory);

        controls.Player.Inventory.performed += OpenInventory_performed;
        controls.Player.ChangeLanguage.performed += ChangeLanguage_performed;

        StaticGameVariables.GetAll();
    }

    /*public void Update()
    {
        if (controls.Player.Hold.ReadValue<float>() > 0f)
        {
            if (progress < 1f)
            {
                progress += Time.unscaledDeltaTime;
            }
        }
        else
        {
            progress = 0f;
        }
    }*/

    private void ChangeLanguage_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        StaticGameVariables.ChangeLanguage(1);
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
}
