using UnityEngine;

public class StartGame : MonoBehaviour
{
    public SceneLoading sceneLoader;
    public Animator transition;
    public string level;
    public float waitTime;
    public void StartNewGame()
    {
        sceneLoader.SwitchToScene(level, "Start");
    }
}