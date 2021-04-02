using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Web : MonoBehaviour
{
    public TMP_InputField loginField;
    public TMP_InputField passwordField;
    public TMP_Text messageField;

    public Button enterLogin;
    public Button playOffline;

    public Translate translate;

    public string message;

    private readonly string errorLogin = "User does not exists";
    private readonly string errorPassword = "Wrong password";

    private void Start()
    {
        //Debug.Log(StaticGameVariables.GetRequest("http://eztix/GetUsers.php"));
        //Debug.Log(StaticGameVariables.UserRegister("TestUnity", "mironov20002000@gmail.com", "121212"));
        enterLogin.onClick.AddListener(Login);
        playOffline.onClick.AddListener(PlayOffline);
    }

    private void Login()
    {
        if (string.IsNullOrWhiteSpace(loginField.text))
        {
            ShowMessageField("Enter login/email");

            return;
        }

        if (string.IsNullOrWhiteSpace(passwordField.text))
        {
            ShowMessageField("Enter password");

            return;
        }

        message = StaticGameVariables.UserLogin(loginField.text, passwordField.text);

        if (message != errorLogin && message != errorPassword)
        {
            PlayOnline(message);
        }
        else
        {
            ShowMessageField(message);
        }
    }

    private void Register()
    {
        //TODO: Add register form with action (Register.php)
    }

    private void ShowMessageField(string text)
    {
        if (Translate.Instance.translationUI.TryGetValue(text, out string value))
        {
            messageField.text = value;

            if (!messageField.isActiveAndEnabled)
            {
                messageField.enabled = true;
            }
        }
    }

    private void PlayOnline(string ID)
    {
        messageField.text = ID;
        SceneLoading.Instance.SwitchToScene("MainMenu", SceneLoading.startAnimationID);
    }

    private void PlayOffline()
    {
        SceneLoading.Instance.SwitchToScene("MainMenu", SceneLoading.startAnimationID);
    }
}
