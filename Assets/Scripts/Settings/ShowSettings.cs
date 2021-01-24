using UnityEngine;

public class ShowSettings : MonoBehaviour
{
    public void Use()
    {
        Settings.Instance.thisCanvas.enabled = true;
        Settings.Instance.defaultCanvas.enabled = true;
        Settings.Instance.SetSlot(Settings.Instance.defaultSlot);
    }
}
