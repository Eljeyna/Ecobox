using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Web : MonoBehaviour
{
    public TMP_Text login;
    public TMP_Text password;

    public Button enterLogin;
    public Button playOffline;

    private void Start()
    {
        //Debug.Log(StaticGameVariables.GetRequest("http://eztix/GetUsers.php"));
        //Debug.Log(StaticGameVariables.UserLogin("__test__testunity@gmail.com", "121212"));
        //Debug.Log(StaticGameVariables.UserRegister("TestUnity", "mironov20002000@gmail.com", "121212"));
        enterLogin.onClick.AddListener(Login);
        playOffline.onClick.AddListener(PlayOffline);
    }

    private void Login()
    {
        Debug.Log(StaticGameVariables.UserLogin(login.text, password.text));
        SceneLoading.Instance.SwitchToScene("MainMenu", SceneLoading.startAnimationID);
    }

    private void PlayOffline()
    {
        SceneLoading.Instance.SwitchToScene("MainMenu", SceneLoading.startAnimationID);
    }
}
