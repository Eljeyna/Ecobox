using System;
using TMPro;
using UnityEngine;

public class TranslateUI : MonoBehaviour
{
    public string[] languages;

    private void Start()
    {
        TMP_Text textUI = gameObject.GetComponent<TMP_Text>();

        if (ReferenceEquals(textUI, null))
        {
            textUI = transform.GetChild(0).GetComponent<TMP_Text>();
        }

        textUI.text = languages[(int)StaticGameVariables.language];
    }
}
