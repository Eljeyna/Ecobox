using UnityEngine;

public class ShowSettings : MonoBehaviour
{
    public void Use()
    {
        Settings.Instance.thisCanvas.sortingOrder = 200;
        Settings.Instance.thisCanvas.enabled = true;
    }
}
