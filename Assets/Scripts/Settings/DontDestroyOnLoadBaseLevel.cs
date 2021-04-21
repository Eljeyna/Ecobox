using UnityEngine;

public class DontDestroyOnLoadBaseLevel : MonoBehaviour
{
    public static DontDestroyOnLoadBaseLevel Instance { get; private set; }

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
