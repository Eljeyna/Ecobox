using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public Animator transition;
    public string level;
    public float waitTime;
    public void StartNewGame() {
        StartCoroutine(StartNewGameCoroutine(waitTime));
    }

    IEnumerator StartNewGameCoroutine(float waitTime)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(level, LoadSceneMode.Single);
    }
}