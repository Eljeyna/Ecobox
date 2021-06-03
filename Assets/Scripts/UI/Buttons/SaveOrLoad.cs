using UnityEngine;

public class SaveOrLoad : MonoBehaviour
{
    public void Save()
    {
        SaveLoadSystem.Instance.Save();
    }

    public async void Load()
    {
        await SaveLoadSystem.Instance.Load();
    }
}
