using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public GameDirector game;
    public float speed;
    private Joystick joystick;
    private Rigidbody2D rb2d;
    private Vector2 movement;

	void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        joystick = game.joystick;
	}

    void FixedUpdate()
    {
        //rb2d.velocity = new Vector2(0f, 0f);
        if (game.canControl)
        {
            float moveHorizontal = Input.GetAxis("Horizontal") + joystick.Horizontal * -1f;
            float moveVertical = Input.GetAxis("Vertical") + joystick.Vertical * -1f;

            if (moveHorizontal > 0f)
            {
                moveHorizontal = 1f;
            }
            else if (moveHorizontal < 0f)
            {
                moveHorizontal = -1f;
            }

            if (moveVertical > 0f)
            {
                moveVertical = 1f;
            }
            else if (moveVertical < 0f)
            {
                moveVertical = -1f;
            }

            movement = new Vector2(moveHorizontal, moveVertical);
            rb2d.velocity = movement * speed * Time.fixedDeltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (game.canControl)
        {
            if (other.gameObject.CompareTag("Citizen"))
            {
                IsTalking talk = other.gameObject.GetComponent<IsTalking>();
                if (talk != null)
                {
                    if (talk.isTalking)
                    {
                        talk.talkButton.SetActive(true);
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Citizen"))
        {
            IsTalking talk = other.gameObject.GetComponent<IsTalking>();
            if (talk != null)
            {
                talk.talkButton.SetActive(false);
            }
        }
    }

    public Rigidbody2D GetPlayerRigidBody()
    {
        return rb2d;
    }
}