using UnityEngine;

public class DontDestroyOnLoadCommon : MonoBehaviour
{
    public static DontDestroyOnLoadCommon Instance { get; private set; }

    private void Awake()
    {
        if (ReferenceEquals(Instance, null))
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }
}
