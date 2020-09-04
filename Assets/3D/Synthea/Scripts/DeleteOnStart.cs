using UnityEngine;

public class DeleteOnStart : MonoBehaviour
{
    private void Start()
    {
        if (SystemInfo.deviceType == DeviceType.Desktop)
            Destroy(gameObject);
    }
}
