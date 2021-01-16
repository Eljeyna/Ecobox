using UnityEngine;

public class ChangeLanguage : MonoBehaviour
{
    public int language;
    public void LanguageChange()
    {
        if (language == 0) language = 1;
        else if (language == 1) language = 0;

        StaticGameVariables.ChangeLanguage(language);
    }
}
