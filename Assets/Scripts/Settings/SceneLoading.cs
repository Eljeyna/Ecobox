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
    private float waitTime;
    private string scene;
    
    public static readonly int startAnimationID = Animator.StringToHash("Start");
    public static readonly int endAnimationID = Animator.StringToHash("End");

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
        }

        if (playAnim)
        {
            anim.SetTrigger(startAnimationID);
        }
        
        Translate.Instance.GetTranslate();
        
        if (GameDirector.Instance)
        {
            GameDirector.Instance.Preload();
        }

        ChangeMusicBetweenScenes();
    }

    private void Update()
    {
        if (waitTime == 0f || waitTime > Time.unscaledTime)
        {
            return;
        }

        waitTime = 0f;
        this.enabled = false;
        LoadLevel(scene);
    }

    public void SwitchToScene(string sceneName, int animId)
    {
        scene = sceneName;
        anim.SetTrigger(animId);
        SwitchToScene();
    }

    public void SwitchToScene()
    {
        waitTime = Time.unscaledTime + 1f;
        StaticGameVariables.PauseGame();
        this.enabled = true;
    }

    public async void LoadLevel(string sceneName)
    {
        if (Player.Instance)
        {
            Player.Instance.inventory.ClearInventory();
            Destroy(Player.Instance.gameObject);
        }

        loadSceneAsync = Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        await loadSceneAsync.Task;
        
        anim.SetTrigger(endAnimationID);
        Translate.Instance.GetTranslate();
        
        if (GameDirector.Instance)
        {
            GameDirector.Instance.Preload();
        }

        ChangeMusicBetweenScenes();
    }

    public void ChangeMusicBetweenScenes()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "MainMenu":
                MusicDirector.Instance.ChangeMusic((int)MusicList.MainMenu);
                break;
            case "Tutorial":
                MusicDirector.Instance.ChangeMusic((int)MusicList.Tutorial);
                break;
            case "Tutorial 01":
                MusicDirector.Instance.ChangeMusic((int)MusicList.Tutorial01);
                break;
        }
    }
}
