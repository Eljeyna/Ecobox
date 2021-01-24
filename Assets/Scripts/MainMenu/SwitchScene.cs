using UnityEngine;

public class SwitchScene : MonoBehaviour
{
    public string level;

    public void Use()
    {
        SceneLoading.Instance.SwitchToScene(level, "Start");
    }
}
