using UnityEngine;
using UnityEngine.InputSystem;

public class CameraSwipeMove : MonoBehaviour
{
    public GameDirector game;
    public float speed;
    public float minClampX;
    public float maxClampX;
    public float minClampY;
    public float maxClampY;

    private Vector2 startPos;
    private Camera cam;
    private Vector2 targetPos;

    private NewInputSystem controls;

    private bool buttonPress;

    private void Awake()
    {
        controls = new NewInputSystem();
    }

    void Start()
    {
        cam = GetComponent<Camera>();
        targetPos = new Vector2(transform.position.x, transform.position.y);

        controls.Player.Attack.performed += attackEvent => buttonPress = true;
        controls.Player.Attack.canceled += attackEvent => buttonPress = false;
    }

    public void OnAttack()
    {
        startPos = cam.ScreenToWorldPoint(Pointer.current.position.ReadValue());
    }

    void Update()
    {
        if (game.canControl)
        {
            if (buttonPress)
            {
                Vector2 pos = new Vector2(cam.ScreenToWorldPoint(Pointer.current.position.ReadValue()).x - startPos.x, cam.ScreenToWorldPoint(Pointer.current.position.ReadValue()).y - startPos.y);
                targetPos = new Vector2(Mathf.Clamp(transform.position.x - pos.x, minClampX, maxClampX), Mathf.Clamp(transform.position.y - pos.y, minClampY, maxClampY));
            }

            transform.position = new Vector3(
                Mathf.Lerp(transform.position.x, targetPos.x, speed * Time.deltaTime),
                Mathf.Lerp(transform.position.y, targetPos.y, speed * Time.deltaTime),
                transform.position.z
            );
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
