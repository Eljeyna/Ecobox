using TMPro;
using UnityEngine;

public class TranslateUIInterface : MonoBehaviour, ITranslate
{
    public string key;

    public void GetTranslate()
    {
        if (gameObject.TryGetComponent(out TMP_Text textUI))
        {
            if (Translate.Instance.translationUI.TryGetValue(key, out string value))
            {
                textUI.text = value;
            }
        }
    }
}
