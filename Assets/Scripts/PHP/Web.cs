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

    private static readonly string errorLogin = "User does not exists";
    private static readonly string errorPassword = "Wrong password";

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
        if (string.IsNullOrWhiteSpace(loginField.text))
        {
            if (Translate.Instance.translationUI.TryGetValue("Enter login/email", out string value))
            {
                messageField.text = value;
            }

            ShowMessageField();
            return;
        }

        if (string.IsNullOrWhiteSpace(passwordField.text))
        {
            if (Translate.Instance.translationUI.TryGetValue("Enter password", out string value))
            {
                messageField.text = value;
            }

            ShowMessageField();
            return;
        }

        message = StaticGameVariables.UserLogin(loginField.text, passwordField.text);

        if (message != errorLogin && message != errorPassword)
        {
            PlayOnline();
        }
        else
        {
            if (Translate.Instance.translationUI.TryGetValue(message, out string value))
            {
                messageField.text = value;
            }

            ShowMessageField();
        }
    }

    private void Register()
    {
        //TODO: Add register form with action (Register.php)
    }

    private void ShowMessageField()
    {
        if (!messageField.isActiveAndEnabled)
        {
            messageField.enabled = true;
        }
    }

    private void PlayOnline()
    {
        SceneLoading.Instance.SwitchToScene("MainMenu", SceneLoading.startAnimationID);
    }

    private void PlayOffline()
    {
        SceneLoading.Instance.SwitchToScene("MainMenu", SceneLoading.startAnimationID);
    }
}
