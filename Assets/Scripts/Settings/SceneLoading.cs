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
    private bool preloaded;
    private float waitTime;
    private string scene;

    public readonly int startAnimationID = Animator.StringToHash("Start");
    public readonly int endAnimationID = Animator.StringToHash("End");

    public readonly string[] biomes = { "Dungeon", "City" };
    public readonly int[] sceneCounters = { 1, 0 };

    private void Awake()
    {
        if (ReferenceEquals(Instance, null))
        {
            Instance = this;

            if (transform.GetChild(0).TryGetComponent(out Animator animator))
            {
                anim = animator;
            }
        }
    }

    private void Start()
    {
        Game.PauseGame();

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

        if (preloaded)
        {
            LoadPreloadedLevel(scene);
            return;
        }

        LoadLevel(scene);
    }

    public void SwitchToScene(string sceneName, int animId, bool preloaded = false)
    {
        this.preloaded = preloaded;
        scene = sceneName;
        anim.SetTrigger(animId);
        SwitchToScene();
    }

    public void SwitchToScene()
    {
        waitTime = Time.unscaledTime + 1f;
        Game.PauseGame();
        this.enabled = true;
    }

    public void PreloadLevel(string sceneName)
    {
        loadSceneAsync = Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Single, false);
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

        AfterLoadLevel();
    }

    public async void LoadPreloadedLevel(string sceneName)
    {
        if (Player.Instance)
        {
            Player.Instance.inventory.ClearInventory();
            Destroy(Player.Instance.gameObject);
        }

        await loadSceneAsync.Task;
        loadSceneAsync.Result.ActivateAsync().completed += AfterPreloadLevel;
    }

    private void AfterPreloadLevel(AsyncOperation obj)
    {
        AfterLoadLevel();
    }

    private void AfterLoadLevel()
    {
        Settings.Instance.blurVolume.SetActive(false);
        anim.SetTrigger(endAnimationID);
        Translate.Instance.GetTranslate();

        if (GameDirector.Instance)
        {
            GameDirector.Instance.Preload();
        }

        if (Player.Instance)
        {
            Game.lastCheckpointPosition = Player.Instance.transform.position;
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
            case "Briefing":
                MusicDirector.Instance.ChangeMusic((int)MusicList.Briefing);
                break;
            case "Tutorial 1":
                MusicDirector.Instance.ChangeMusic((int)MusicList.Tutorial01);
                break;
            case "Dungeon 1":
                MusicDirector.Instance.ChangeMusic((int)MusicList.Dungeon);
                break;
            default:
                MusicDirector.Instance.StopMusic();
                break;
        }
    }
}
