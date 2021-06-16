using TMPro;
using UnityEngine;

public class BriefingSystem : MonoBehaviour
{
    public static BriefingSystem Instance { get; private set; }
    public Animator animationText;
    public TranslateBriefing briefingScript;
    public TMP_Text briefing;
    public GameObject invisibleButton;
    public int currentBriefing;
    public bool readyForInput;

    private float waitTime;
    private float waitSwitchTime;

    private void Awake()
    {
        Instance = this;
        waitTime = Time.unscaledTime + 2f;
        SceneLoading.Instance.PreloadLevel("Tutorial 1");
#if UNITY_ANDROID || UNITY_IOS
        GameUI.Instance.invisibleButton.gameObject.SetActive(true);
#endif
    }

    private void Update()
    {
        if (waitTime > Time.unscaledTime)
        {
            return;
        }

        if (readyForInput)
        {
            if (Game.GetInput())
            {
                UpdateBriefing(currentBriefing + 1);
            }

            return;
        }

        animationText.SetInteger(Game.animationKeyID, 0);
        
        if (briefingScript.briefing.TryGetValue($"Mission {currentBriefing}", out string text))
        {
            briefing.text = text;
            waitSwitchTime = Time.unscaledTime + 1f;
            invisibleButton.SetActive(true);
            //this.enabled = false;
            readyForInput = true;
            return;
        }

#if UNITY_ANDROID || UNITY_IOS
        GameUI.Instance.invisibleButton.gameObject.SetActive(false);
#endif
        this.enabled = false;
        readyForInput = false;
        briefing.text = string.Empty;
        SceneLoading.Instance.SwitchToScene("Tutorial 1", SceneLoading.Instance.startAnimationID, true);
    }

    public void UpdateBriefing(int newBriefing)
    {
        if (waitSwitchTime > Time.unscaledTime)
        {
            return;
        }

        currentBriefing = newBriefing;
        waitTime = Time.unscaledTime + 1f;
        animationText.SetInteger(Game.animationKeyID, 1);
        invisibleButton.SetActive(false);
        //this.enabled = true;
        readyForInput = false;
    }
}
