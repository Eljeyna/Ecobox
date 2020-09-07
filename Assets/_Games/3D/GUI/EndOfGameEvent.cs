using UnityEngine;
using UnityEngine.SceneManagement;

public class EndOfGameEvent : MonoBehaviour
{
    public void BackToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
