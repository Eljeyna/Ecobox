using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoading : MonoBehaviour
{
    public Image loadingProgressBar;

    private static SceneLoading instance;
    private static bool playAnim = false;
    private Animator anim;
    private AsyncOperation loadingSceneOperation;

    public void SwitchToScene(string sceneName, string animId)
    {
        Application.backgroundLoadingPriority = ThreadPriority.High;
        instance.anim.SetTrigger(animId);
        StartCoroutine(AsyncLoad(sceneName));
    }

    IEnumerator AsyncLoad(string sceneName)
    {
        instance.loadingSceneOperation = SceneManager.LoadSceneAsync(sceneName);
        while (!instance.loadingSceneOperation.isDone)
        {
            float progress = loadingSceneOperation.progress / 0.9f;
            loadingProgressBar.fillAmount = progress;
            yield return null;
        }
    }

    void Start()
    {
        instance = this;
        anim = GetComponent<Animator>();

        if (playAnim)
        {
            anim.SetTrigger("Start");
        }
    }
}
