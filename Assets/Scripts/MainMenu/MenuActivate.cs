using UnityEngine;

public class MenuActivate : MonoBehaviour
{
    public CanvasGroup canvasGroup;

    private void Awake()
    {
        Settings.Instance.eventExit.OnExit += EventExit;
    }

    private void EventExit(object sender, System.EventArgs e)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }
}
