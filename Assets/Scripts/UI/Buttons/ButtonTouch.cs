using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonTouch : MonoBehaviour, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Player.Instance.touch = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            return;
        }

        Player.Instance.touch = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Player.Instance.touch = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Player.Instance.touch = false;
    }
}
