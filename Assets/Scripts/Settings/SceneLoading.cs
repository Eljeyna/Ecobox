using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class SceneLoading : MonoBehaviour
{
    public static SceneLoading Instance { get; private set; }

    private Animator anim;
    private AsyncOperationHandle<SceneInstance> loadSceneAsync;

    private bool playAnim = false;
    
    private static readonly int startAnimationID = Animator.StringToHash("Start");
    private static readonly int endAnimationID = Animator.StringToHash("End");

    private void Start()
    {
        StaticGameVariables.PauseGame();
        
        if (ReferenceEquals(Instance, null))
        {
            Instance = this;
            if (transform.GetChild(0).TryGetComponent(out Animator animator))
            {
                anim = animator;
            }
            
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (playAnim)
        {
            anim.SetTrigger(startAnimationID);
        }
        
        Translate.Instance.GetTranslate();
        
        if (GameDirector.Instance != null)
        {
            GameDirector.Instance.Preload();
        }
    }

    public void SwitchToScene(string sceneName, string animId)
    {
        anim.SetTrigger(animId);
        LoadLevel(sceneName);
    }

    public async void LoadLevel(string sceneName)
    {
        StaticGameVariables.PauseGame();
        
        if (Player.Instance != null)
        {
            Player.Instance.inventory.ClearInventory();
            Destroy(Player.Instance.gameObject);
        }

        loadSceneAsync = Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        await loadSceneAsync.Task;
        
        anim.SetTrigger(endAnimationID);
        Translate.Instance.GetTranslate();
        
        if (GameDirector.Instance != null)
        {
            GameDirector.Instance.Preload();
        }
    }
}
