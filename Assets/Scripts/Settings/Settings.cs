using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Newtonsoft.Json.Utilities;

public class Settings : MonoBehaviour
{
    public static Settings Instance { get; private set; }

    public bool gameIsLoaded;

    /*public TMP_Dropdown dropdownResolutions;
    public TMP_Dropdown dropdownFullscreenMode;*/
    public Slider sliderMasterVolume;
    public Slider sliderSFXVolume;
    public Slider sliderMusicVolume;
    public Slider sliderGUIVolume;

    public Slider sliderCameraZoom;

    public AudioMixer audioMixer;
    public Canvas thisCanvas;
    public CanvasGroup canvasGroup;
    public GameObject blurVolume;

    public EventExit eventExit;

    private void Awake()
    {
        if (ReferenceEquals(Instance, null))
        {
            Instance = this;
        }

        //For AOT Compiler (fix errors)
        AotHelper.EnsureList<string[]>();
        AotHelper.EnsureDictionary<string, string>();
        AotHelper.EnsureList<QuestsTasksStruct>();
        AotHelper.EnsureList<QuestTask>();
        AotHelper.EnsureList<CompletedQuestsID>();
        AotHelper.EnsureList<AnswersArray>();
        AotHelper.EnsureList<AnswersFile>();
        AotHelper.EnsureList<Sentences>();
        AotHelper.EnsureList<Saveable>();

        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }

    private void Start()
    {
        /*dropdownResolutions.ClearOptions();
        dropdownFullscreenMode.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            string option = Screen.resolutions[i].width + " x " + Screen.resolutions[i].height;
            options.Add(option);

            if (Screen.resolutions[i].width == Screen.currentResolution.width && Screen.resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        dropdownResolutions.AddOptions(options);
        dropdownResolutions.value = PlayerPrefs.GetInt("Resolution", currentResolutionIndex);
        dropdownResolutions.RefreshShownValue();

        options.Clear();
        options.Add("FullScreen");
        options.Add("Borderless");
        options.Add("Maximized");
        options.Add("Windowed");

        dropdownFullscreenMode.AddOptions(options);
        dropdownFullscreenMode.value = (int)Screen.fullScreenMode;
        dropdownResolutions.RefreshShownValue();*/

        sliderMasterVolume.value = PlayerPrefs.GetFloat("MasterVolume", 0f);
        audioMixer.SetFloat("masterVolume", PlayerPrefs.GetFloat("MasterVolume", 0f));

        sliderSFXVolume.value = PlayerPrefs.GetFloat("SFXVolume", 0f);
        audioMixer.SetFloat("sfxVolume", PlayerPrefs.GetFloat("SFXVolume", 0f));

        sliderMusicVolume.value = PlayerPrefs.GetFloat("MusicVolume", 0f);
        audioMixer.SetFloat("musicVolume", PlayerPrefs.GetFloat("MusicVolume", 0f));

        sliderGUIVolume.value = PlayerPrefs.GetFloat("GUIVolume", 0f);
        audioMixer.SetFloat("guiVolume", PlayerPrefs.GetFloat("GUIVolume", 0f));

        sliderCameraZoom.value = PlayerPrefs.GetFloat("ZoomAmount", 3f);
        if (Player.Instance != null)
        {
            Player.Instance.zoomAmount = 0.2f + (0.2f * sliderCameraZoom.value);
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = Screen.resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreenMode);
        PlayerPrefs.SetInt("Resolution", resolutionIndex);
    }

    public void SetFullscreenMode(int fullscreenMode)
    {
        Screen.fullScreenMode = (FullScreenMode)fullscreenMode;
        PlayerPrefs.SetInt("Fullscreen Mode", fullscreenMode);
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", volume);
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("sfxVolume", volume);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", volume);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetGUIVolume(float volume)
    {
        audioMixer.SetFloat("guiVolume", volume);
        PlayerPrefs.SetFloat("GUIVolume", volume);
    }

    public void SetCameraZoom(float zoom)
    {
        PlayerPrefs.SetFloat("ZoomAmount", zoom);

        if (Player.Instance != null)
        {
            Player.Instance.zoomAmount = 0.2f + (0.2f * zoom);
        }
    }

    public void ShowSettings()
    {
        thisCanvas.enabled = true;
        thisCanvas.sortingOrder = 200;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    public void HideSettings()
    {
        thisCanvas.enabled = false;
        thisCanvas.sortingOrder = 0;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
        eventExit.Call();
    }
}
