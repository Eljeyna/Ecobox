using UnityEngine;

public class LoadGame : MonoBehaviour
{
    public void Use()
    {
        Settings.Instance.gameIsLoaded = true;
    }
}
