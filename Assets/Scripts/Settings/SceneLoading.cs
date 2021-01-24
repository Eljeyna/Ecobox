using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using System.Threading.Tasks;

public class SceneLoading : MonoBehaviour
{
    public static SceneLoading Instance { get; private set; }

    public Image loadingProgressBar;
    public Animator anim;

    private AsyncOperationHandle<SceneInstance> loadSceneAsync;
    private bool playAnim = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (playAnim)
        {
            anim.SetTrigger("Start");
        }
    }

    public void SwitchToScene(string sceneName, string animId)
    {
        Instance.anim.SetTrigger(animId);
        _ = LoadLevel(sceneName);

    }

    public async Task LoadLevel(string sceneName)
    {
        loadSceneAsync = Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        /*float progress = loadSceneAsync.PercentComplete;
        loadingProgressBar.fillAmount = progress;*/
        await loadSceneAsync.Task;
        StaticGameVariables.ResumeGame();
        anim.SetTrigger("End");
    }
}
