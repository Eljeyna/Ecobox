using UnityEngine;

public class ChangeLanguage : MonoBehaviour
{
    public int language;

    public void LanguageChange()
    {
        StaticGameVariables.ChangeLanguage(language);
    }
}
