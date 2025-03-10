using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Web : MonoBehaviour
{
    public string level;

    public Canvas canvasSignIn;
    public CanvasGroup canvasGroup;
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
    private readonly string errorConnection = "Connection failed";

    private void Start()
    {
        //Debug.Log(StaticGameVariables.GetRequest("http://eztix/GetUsers.php"));
        SceneLoading.Instance.PreloadLevel(level);
        enterLogin.onClick.AddListener(Login);
        enterRegister.onClick.AddListener(Register);
        playOffline.onClick.AddListener(Play);
    }

    private async void Login()
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

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

        message = await Game.UserLogin(loginField.text, passwordField.text);

        if (message != string.Empty && message != errorLogin && message != errorPassword && !message.Contains(errorConnection))
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
            PlayOnline(message);
        }
        else
        {
            ShowMessageField(message);
        }
    }

    private async void Register()
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;

        if (string.IsNullOrWhiteSpace(loginFieldRegister.text))
        {
            ShowMessageField("Enter login/email");

            return;
        }

        if (string.IsNullOrWhiteSpace(emailRegister.text))
        {
            ShowMessageField("Enter email");

            return;
        }
        
        if (string.IsNullOrWhiteSpace(passwordFieldRegister.text))
        {
            ShowMessageField("Enter password");

            return;
        }
        
        if (passwordFieldRegister.text != passwordConfirmFieldRegister.text)
        {
            ShowMessageField("Passwords are different");

            return;
        }

        message = await Game.UserRegister(loginFieldRegister.text, emailRegister.text, passwordFieldRegister.text);

        if (message != string.Empty && message == confirmRegisterMessage)
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
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

        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;
    }

    private void PlayOnline(string ID)
    {
        Game.accountID = ID;
        Play();
    }

    private void Play()
    {
        canvasGroup.interactable = false;
        messageField.enabled = false;
        SceneLoading.Instance.SwitchToScene(level, SceneLoading.Instance.startAnimationID, true);
    }
}
