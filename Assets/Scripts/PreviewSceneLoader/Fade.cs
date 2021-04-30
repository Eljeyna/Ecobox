using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using System.Threading.Tasks;

public class Fade : MonoBehaviour
{
    public AssetReference level;
    public float time = 4f;

    private Animator fade;
    private bool m_ButtonPressed;

    private AsyncOperationHandle<SceneInstance> loadSceneAsync;

    private void Awake()
    {
        loadSceneAsync = Addressables.LoadSceneAsync(level, LoadSceneMode.Single, false);
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

    private void Update()
    {
        if (m_ButtonPressed)
        {
            StopCoroutine(End());
            WaitLoadLevel();
        }
    }

    private IEnumerator End()
    {
        yield return new WaitForSeconds(time);
        fade.SetTrigger(SceneLoading.startAnimationID);
        yield return new WaitForSeconds(1.0f);
        WaitLoadLevel();
    }

    private async void WaitLoadLevel()
    {
        int delay = 5;

        if (loadSceneAsync.IsValid())
        {
            while (loadSceneAsync.PercentComplete != 1f)
            {
                await Task.Delay(delay);
            }

            loadSceneAsync.Result.ActivateAsync();
        }
    }
}
