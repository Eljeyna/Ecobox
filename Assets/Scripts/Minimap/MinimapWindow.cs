using UnityEngine;

public class MinimapWindow : MonoBehaviour
{
    public static MinimapWindow Instance;

    private void Awake()
    {
        Instance = this;
    }

    public static void Show()
    {
        Instance.gameObject.SetActive(true);
    }

    public static void Hide()
    {
        Instance.gameObject.SetActive(false);
    }
}
