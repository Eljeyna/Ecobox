using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonTouch : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Player.Instance.touch = false;
        Player.Instance.buttonTouch = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        Player.Instance.touch = false;
        Player.Instance.buttonTouch = false;
    }
}
