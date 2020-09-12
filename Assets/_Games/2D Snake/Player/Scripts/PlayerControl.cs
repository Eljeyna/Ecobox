using TMPro;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float speed = 4f;
    public TMP_Text scoreText;

    [HideInInspector] public GameObject trash;
    [HideInInspector] public Joystick joystick;
    [HideInInspector] public Rigidbody2D rb2d;
    [HideInInspector] public Vector2 moving;

    private int score;
    private int scoreAdd;
    private Vector2 moveVelocity;
    private NewInputSystem controls;

    private void Awake()
    {
        controls = new NewInputSystem();
    }

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();

        controls.Player.Movement.performed += movementEvent => moving = movementEvent.ReadValue<Vector2>();
        controls.Player.Movement.canceled += movementEvent => moving = Vector2.zero;
    }

    private void FixedUpdate()
    {
        if (moveVelocity != Vector2.zero)
        {
            //rb2d.velocity = moving * speed * Time.fixedDeltaTime;
            rb2d.MovePosition(rb2d.position + moveVelocity * Time.fixedDeltaTime);
            if (trash != null)
                trash.transform.position = rb2d.position;
        }

    }

    private void Update()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (joystick.Direction != Vector2.zero)
            moving = new Vector2(joystick.Direction.x, joystick.Direction.y);
#endif

        moveVelocity = moving.normalized * speed;
        if (moveVelocity != Vector2.zero)
            RotateGameObject(rb2d.position + moveVelocity);

#if UNITY_ANDROID || UNITY_IOS
        if (joystick.Direction != Vector2.zero)
            moving = Vector2.zero;
#endif
    }

    private void RotateGameObject(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));
        transform.rotation = rotation;
    }

    private void UpdatePlayerScore()
    {
        score += scoreAdd;
        scoreText.text = "Score: " + score;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 31) // Layer: Wall
        {
            transform.position = -transform.position;
            return;
        }
        else if (other.gameObject.layer == 8) // Layer: Entities
        {
            BaseTag trashEntity = other.GetComponent<BaseTag>();
            if (trash != null && (trashEntity.entityTag & Tags.EntityTags.FL_TRASHBOX) != 0)
            {
                SpawnerSnake spawnerSnake = trash.transform.parent.GetComponent<SpawnerSnake>();
                if (spawnerSnake != null)
                {
                    spawnerSnake.waveEntities.Remove(trash);
                    spawnerSnake.currentWaveCount--;
                }

                Destroy(trash);

                if ((trash.GetComponent<BaseTrashTag>().trashTag & other.GetComponent<BaseTrashTag>().trashTag) != 0)
                    UpdatePlayerScore();

                scoreAdd = 0;
            }
            else
            {
                if ((trashEntity.entityTag & Tags.EntityTags.FL_PICKUP) != 0)
                {
                    if (trash == null)
                    {
                        other.enabled = false;
                        trash = other.gameObject;
                        scoreAdd++;
                    }
                    else if ((trash.GetComponent<BaseTrashTag>().trashTag & other.GetComponent<BaseTrashTag>().trashTag) != 0)
                    {
                        SpawnerSnake spawnerSnake = other.transform.parent.GetComponent<SpawnerSnake>();
                        if (spawnerSnake != null)
                        {
                            spawnerSnake.waveEntities.Remove(other.gameObject);
                            spawnerSnake.currentWaveCount--;
                        }

                        Destroy(other.gameObject);
                        scoreAdd++;
                    }
                }
            }
        }
    }
}