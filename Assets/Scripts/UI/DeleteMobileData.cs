using UnityEngine;

public class DeleteMobileData : MonoBehaviour
{
    private void Start()
    {
#if UNITY_ANDROID || UNITY_IOS
        Destroy(this);
#else
        Destroy(gameObject);
#endif
    }
}
