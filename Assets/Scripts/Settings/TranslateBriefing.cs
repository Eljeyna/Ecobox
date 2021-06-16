using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class TranslateBriefing : MonoBehaviour, ITranslate
{
    public Dictionary<string, string> briefing;
    
    public async Task GetTranslate()
    {
        StringBuilder sb = new StringBuilder(await Game.GetAsset(Path.Combine("Localization", Game.languageKeys[(int)Game.language], "Text", "Briefing.json")));

#if UNITY_ANDROID && !UNITY_EDITOR_LINUX
        if (sb.ToString() == string.Empty)
        {
            return;
        }
        
        briefing = JsonConvert.DeserializeObject<Dictionary<string, string>>(sb.ToString());
#else
        if (!File.Exists(sb.ToString()))
        {
            return;
        }

        briefing = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(sb.ToString()));
#endif
    }
}
