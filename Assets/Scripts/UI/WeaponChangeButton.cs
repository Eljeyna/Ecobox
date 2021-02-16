using UnityEngine;
using UnityEngine.EventSystems;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class WeaponChangeButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        Player.Instance.touch = false;
        Player.Instance.weaponChangeTouch = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
#if UNITY_ANDROID || UNITY_IOS
        if (Touch.activeFingers.Count == 2)
        {
            return;
        }
#endif
        
        Player.Instance.touch = false;
        Player.Instance.weaponChangeTouch = false;
    }
}
