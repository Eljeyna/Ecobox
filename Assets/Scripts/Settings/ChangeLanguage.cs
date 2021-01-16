using UnityEngine;

public class ChangeLanguage : MonoBehaviour
{
    public int language;

    private void Start()
    {
        language = PlayerPrefs.GetInt("Language", 0);

        if (language != 0)
        {
            StaticGameVariables.ChangeLanguage(language);
        }
    }

    public void LanguageChange()
    {
        if (language == 0) language = 1;
        else if (language == 1) language = 0;

        StaticGameVariables.ChangeLanguage(language);
    }
}
