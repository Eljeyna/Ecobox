using UnityEngine;

public class DeleteOnStartStandalone : MonoBehaviour
{
    private void Start()
    {
#if !UNITY_ANDROID && !UNITY_IOS
        Destroy(gameObject);
#else
        Destroy(this);
#endif
    }
}
