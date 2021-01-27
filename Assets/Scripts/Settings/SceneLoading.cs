using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class SceneLoading : MonoBehaviour
{
    public static SceneLoading Instance { get; private set; }

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
        LoadLevel(sceneName);

    }

    public async void LoadLevel(string sceneName)
    {
        if (Player.Instance != null)
        {
            Player.Instance.inventory.ClearInventory();
        }

        loadSceneAsync = Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        await loadSceneAsync.Task;
        StaticGameVariables.ResumeGame();
        anim.SetTrigger("End");
    }
}
