using UnityEngine;

public class ChangeLanguage : MonoBehaviour
{
    public int language;

    public void LanguageChange()
    {
        Game.ChangeLanguage(language);
    }
}
