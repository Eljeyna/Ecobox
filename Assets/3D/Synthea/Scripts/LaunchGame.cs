using TMPro;
using UnityEngine;

public class LaunchGame : MonoBehaviour
{
    public TMP_Text text;

    public void Launch()
    {
        GameDirector3D.StartGame();
        Destroy(text.gameObject);
        Destroy(gameObject);
    }
}
