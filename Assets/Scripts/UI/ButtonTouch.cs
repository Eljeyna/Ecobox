using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonTouch : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Player.Instance.touch = false;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        Player.Instance.touch = false;
    }
}
