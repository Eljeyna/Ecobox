using UnityEngine;

public class SwitchScene : MonoBehaviour
{
    public string level;
    private string animID = "Start";

    public void Use()
    {
        SceneLoading.Instance.SwitchToScene(level, animID);
    }
}
