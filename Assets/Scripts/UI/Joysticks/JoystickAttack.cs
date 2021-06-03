using UnityEngine;
using UnityEngine.EventSystems;

public class JoystickAttack : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Joystick joystick;

    private bool attack;

    private void Update()
    {
        if (!attack)
        {
            return;
        }
        
        if (joystick.DeadZone < joystick.Direction.magnitude)
        {
            if (Game.isPause || Player.Instance.state != EntityState.Normal)
            {
                return;
            }

            Player.Instance.targetDirection = joystick.Direction.normalized;
            Player.Instance.Attack();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        attack = true;
        Player.Instance.touch = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        attack = false;
        Player.Instance.touch = false;
    }
}
