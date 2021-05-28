using System.IO;
using System.Text;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

public class AdvicesSystem : MonoBehaviour, ITranslate
{
    public TMP_Text textField;
    public Animator animationData;
    public string[] data;
    public int previousAdvice;
    public int advice;

    private float timeToChangeAdvice = 15f;
    private float timeFade = 1f;
    
    private float nextTimeAdvice;
    private float nextTimeChangeAdvice;

    private void Awake()
    {
        textField.text = data[advice];
        nextTimeChangeAdvice = Time.unscaledTime + timeFade;
        nextTimeAdvice = Time.unscaledTime + timeToChangeAdvice;
    }

    private void Update()
    {
        if (nextTimeChangeAdvice != 0f && nextTimeChangeAdvice <= Time.unscaledTime)
        {
            textField.text = data[advice];
            nextTimeChangeAdvice = 0f;
            animationData.SetInteger(Game.animationKeyID, 0);
            return;
        }
        
        if (nextTimeAdvice > Time.unscaledTime)
        {
            return;
        }
        
        Game.GetRandom();
        previousAdvice = advice;
        advice = (int)(Game.random * (data.Length - 1) + 0.5f);

        if (advice == previousAdvice)
        {
            if (advice == 0)
            {
                advice = data.Length - 1;
            }
            else if (advice == data.Length - 1)
            {
                advice = 0;
            }
            else
            {
                Game.GetRandom();
                advice = (int)(Game.random * (advice - 1));
            }
        }
        
        animationData.SetInteger(Game.animationKeyID, 1);

        nextTimeAdvice = Time.unscaledTime + timeToChangeAdvice;
        nextTimeChangeAdvice = Time.unscaledTime + timeFade;
    }

    public void GetTranslate()
    {
        StringBuilder sb = new StringBuilder(Game.GetAsset(Path.Combine("Localization", Game.languageKeys[(int) Game.language], "Advices.json")));

#if UNITY_ANDROID && !UNITY_EDITOR_LINUX
        if (sb.ToString() == string.Empty)
        {
            return;
        }
        
        translationUI = JsonConvert.DeserializeObject<string[]>(sb.ToString());
#else
        if (!File.Exists(sb.ToString()))
        {
            return;
        }
#endif

        data = JsonConvert.DeserializeObject<string[]>(File.ReadAllText(sb.ToString()));
        textField.text = data[advice];
    }
}
