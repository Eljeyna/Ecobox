using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class TranslateUIInterface : MonoBehaviour, ITranslate
{
    public string key;

    public async Task GetTranslate()
    {
        if (gameObject.TryGetComponent(out TMP_Text textUI))
        {
            if (Translate.Instance.translationUI.TryGetValue(key, out string value))
            {
                await Task.FromResult(textUI.text = value);
            }
        }
    }
}
