using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;

public class Fade : MonoBehaviour
{
    private Animator fade;
    public float time = 4f;

    private bool m_ButtonPressed;

    private void Awake()
    {
        InputSystem.onEvent +=
        (eventPtr, device) =>
        {
            if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>())
                return;
            var controls = device.allControls;
            var buttonPressPoint = InputSystem.settings.defaultButtonPressPoint;
            for (var i = 0; i < controls.Count; ++i)
            {
                var control = controls[i] as ButtonControl;
                if (control == null || control.synthetic || control.noisy)
                    continue;
                if (control.ReadValueFromEvent(eventPtr, out var value) && value >= buttonPressPoint)
                {
                    m_ButtonPressed = true;
                    break;
                }
            }
        };
    }

    private void OnAttack()
    {
        m_ButtonPressed = true;
    }

    private void Start()
    {
        fade = gameObject.GetComponent<Animator>();
        StartCoroutine(End());
    }

    private async void Update()
    {
        if (m_ButtonPressed)
        {
            StopCoroutine(End());
            await ChangeLevel("MainMenu");
        }
    }

    IEnumerator End()
    {
        yield return new WaitForSeconds(time);
        fade.SetTrigger("Start");
        yield return new WaitForSeconds(1.0f);
        _ = ChangeLevel("MainMenu");
    }

    private async Task ChangeLevel(string sceneName)
    {
        var loadSceneAsync = Addressables.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        await loadSceneAsync.Task;
    }
}
