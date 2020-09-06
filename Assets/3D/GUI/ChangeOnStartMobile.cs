using TMPro;
using UnityEngine;

public class ChangeOnStartMobile : MonoBehaviour
{
#if UNITY_ANDROID || UNITY_IOS
    private TMP_Text text;
#endif
    void Start()
    {
#if UNITY_ANDROID || UNITY_IOS
        text = GetComponent<TMP_Text>();
        text.text = "Чтобы атаковать, нажмите на экран\nTo attack you should use touch screen";
#else
        Destroy(this);
#endif
    }
}
