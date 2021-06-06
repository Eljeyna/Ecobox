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

    private float waitTime;
    private float waitSwitchTime;

    private void Awake()
    {
        Instance = this;
        waitTime = Time.unscaledTime + 2f;
    }

    private void Update()
    {
        if (waitTime > Time.unscaledTime)
        {
            return;
        }

        animationText.SetInteger(Game.animationKeyID, 0);
        
        if (briefingScript.briefing.TryGetValue($"Mission {currentBriefing}", out string text))
        {
            briefing.text = text;
            waitSwitchTime = Time.unscaledTime + 1f;
            invisibleButton.SetActive(true);
            this.enabled = false;
            return;
        }

        this.enabled = false;
        briefing.text = string.Empty;
        SceneLoading.Instance.SwitchToScene("Tutorial 01", SceneLoading.startAnimationID);
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
        this.enabled = true;
    }
}
