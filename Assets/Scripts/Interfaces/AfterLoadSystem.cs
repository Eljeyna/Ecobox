using UnityEngine;

public class AfterLoadSystem : MonoBehaviour
{
    public static AfterLoadSystem Instance { get; private set; }

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
    }

    public void Load()
    {
        foreach (var monoBehaviour in FindObjectsOfType<MonoBehaviour>())
        {
            if (monoBehaviour is IAfterSaveState persist)
            {
                persist.Load();
            }
        }
    }
}

internal interface IAfterSaveState
{
    void Load();
}
