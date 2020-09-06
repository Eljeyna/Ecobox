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
        text.text = "Чтобы атаковать, нажмите на кнопку в правом углу\nTo attack you should use button on the right corner";
#else
        Destroy(this);
#endif
    }
}
