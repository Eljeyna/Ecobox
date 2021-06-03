using UnityEngine;

public class MenuActivate : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    private void EventExit(object sender, System.EventArgs e)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    private void OnEnable()
    {
        Settings.Instance.eventExit.OnExit += EventExit;
    }
    
    private void OnDisable()
    {
        Settings.Instance.eventExit.OnExit -= EventExit;
    }
}
