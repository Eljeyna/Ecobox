using TMPro;
using UnityEngine;

public class TranslateUI : MonoBehaviour
{
    public string[] languages;

    private void Start()
    {
        if (StaticGameVariables.language != StaticGameVariables.Language.Russian)
        {
            TMP_Text textUI = transform.GetChild(0).GetComponent<TMP_Text>();
            textUI.text = languages[(int)StaticGameVariables.language];
        }
    }
}
