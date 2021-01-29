using UnityEngine;

public class SaveOrLoad : MonoBehaviour
{
    public void Save()
    {
        SaveLoadSystem.Instance.Save();
    }

    public void Load()
    {
        SaveLoadSystem.Instance.Load();
    }
}
