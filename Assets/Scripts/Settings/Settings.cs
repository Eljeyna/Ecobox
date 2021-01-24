using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public static Settings Instance { get; private set; }

    /*public TMP_Dropdown dropdownResolutions;
    public TMP_Dropdown dropdownFullscreenMode;*/
    public Slider sliderMasterVolume;
    public Slider sliderSFXVolume;
    public Slider sliderMusicVolume;
    public Slider sliderGUIVolume;

    public Slider sliderCameraZoom;

    public AudioMixer audioMixer;
    public Canvas thisCanvas;
    public Canvas defaultCanvas;

    public Color colorSlot;

    [HideInInspector] public Image defaultSlot;
    [HideInInspector] public Image slotSelected;

    private void Awake()
    {
        StaticGameVariables.InitializeLanguage();
    }

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;

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

        defaultSlot = GameObject.Find("SoundCategory").GetComponent<Image>();
        slotSelected = GameObject.Find("SoundCategory").GetComponent<Image>();
        slotSelected.color = colorSlot;
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

    public void SetSlot(Image slot)
    {
        if (slotSelected == slot)
        {
            return;
        }

        slotSelected.color = Color.white;
        slotSelected = slot;
        slotSelected.color = colorSlot;
    }
}
