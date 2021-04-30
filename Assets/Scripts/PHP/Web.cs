using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Web : MonoBehaviour
{
    public Canvas canvasSignIn;
    public Canvas canvasSignUp;

    public TMP_InputField loginField;
    public TMP_InputField loginFieldRegister;

    public TMP_InputField passwordField;
    public TMP_InputField passwordFieldRegister;
    public TMP_InputField passwordConfirmFieldRegister;

    public TMP_InputField emailRegister;

    public TMP_Text messageField;

    public Button enterLogin;
    public Button enterRegister;

    public Button playOffline;

    public Translate translate;

    public string message;

    private readonly string errorLogin = "User does not exists";
    private readonly string errorPassword = "Wrong password";
    private readonly string confirmRegisterMessage = "All done";

    private void Start()
    {
        //Debug.Log(StaticGameVariables.GetRequest("http://eztix/GetUsers.php"));
        enterLogin.onClick.AddListener(Login);
        enterRegister.onClick.AddListener(Register);
        playOffline.onClick.AddListener(PlayOffline);
    }

    private void Login()
    {
        if (string.IsNullOrWhiteSpace(loginField.text))
        {
            ShowMessageField("Enter login/email");

            return;
        }
        else if (string.IsNullOrWhiteSpace(passwordField.text))
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
        if (string.IsNullOrWhiteSpace(loginFieldRegister.text))
        {
            ShowMessageField("Enter login/email");

            return;
        }
        else if (string.IsNullOrWhiteSpace(emailRegister.text))
        {
            ShowMessageField("Enter email");

            return;
        }
        else if (string.IsNullOrWhiteSpace(passwordFieldRegister.text))
        {
            ShowMessageField("Enter password");

            return;
        }
        else if (passwordFieldRegister.text != passwordConfirmFieldRegister.text)
        {
            ShowMessageField("Passwords are different");

            return;
        }

        message = StaticGameVariables.UserRegister(loginFieldRegister.text, emailRegister.text, passwordFieldRegister.text);

        if (message == confirmRegisterMessage)
        {
            canvasSignUp.enabled = false;
            canvasSignIn.enabled = true;
        }
        else
        {
            ShowMessageField(message);
        }
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
        messageField.enabled = false;
        messageField.text = ID;
        SceneLoading.Instance.SwitchToScene("MainMenu", SceneLoading.startAnimationID);
    }

    private void PlayOffline()
    {
        messageField.enabled = false;
        SceneLoading.Instance.SwitchToScene("MainMenu", SceneLoading.startAnimationID);
    }
}
