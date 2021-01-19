using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

public class SceneLoading : MonoBehaviour
{
    public Image loadingProgressBar;

    private UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<UnityEngine.ResourceManagement.ResourceProviders.SceneInstance> loadSceneAsync;
    private static SceneLoading instance;
    private static bool playAnim = false;
    private Animator anim;

    public void SwitchToScene(string sceneName, string animId)
    {
        instance.anim.SetTrigger(animId);
        _ = LoadLevel(sceneName);
    }

    public async Task LoadLevel(string sceneName)
    {
        loadSceneAsync = Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        /*float progress = loadSceneAsync.PercentComplete;
        loadingProgressBar.fillAmount = progress;*/
        await loadSceneAsync.Task;
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
