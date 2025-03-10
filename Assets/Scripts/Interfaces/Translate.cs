using System.IO;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using static Game;
using System.Threading.Tasks;

public class Translate : MonoBehaviour
{
    public static Translate Instance { get; private set; }
    public Dictionary<string, string> translationUI = new Dictionary<string, string>();

    private int language = -1;

    private void Awake()
    {
        if (ReferenceEquals(Instance, null))
        {
            Instance = this;
        }

        InitializeLanguage();
    }

    public void Translation()
    {
        GetTranslate();
    }

    public async void GetTranslate()
    {
        if (language != (int)Game.language)
        {
            language = (int)Game.language;
        }
        
        StringBuilder sb = new StringBuilder(await GetAsset(Path.Combine("Localization", languageKeys[language], "UI.json")));

#if UNITY_ANDROID && !UNITY_EDITOR_LINUX
        if (sb.ToString() == string.Empty)
        {
            return;
        }
        
        translationUI = JsonConvert.DeserializeObject<Dictionary<string, string>>(sb.ToString());
#else
        if (!File.Exists(sb.ToString()))
        {
            return;
        }

        //Debug.Log(new DirectoryInfo(GetAsset(Path.Combine("Localization", languageKeys[language], "UI.json"))).FullName);
        translationUI = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(sb.ToString()));
#endif
        if (translationUI == null)
        {
            return;
        }
    
        foreach (var monoBehaviour in FindObjectsOfType<MonoBehaviour>())
        {
            if (monoBehaviour is ITranslate persist)
            {
                _ = persist.GetTranslate();
            }
        }
    }
}

internal interface ITranslate
{
    Task GetTranslate();
}
