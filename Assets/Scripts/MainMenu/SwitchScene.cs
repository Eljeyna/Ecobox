using UnityEngine;

public class SwitchScene : MonoBehaviour
{
    public SceneLoading sceneLoader;
    public Animator transition;
    public string level;
    public float waitTime;
    public void Use()
    {
        sceneLoader.SwitchToScene(level, "Start");
    }
}