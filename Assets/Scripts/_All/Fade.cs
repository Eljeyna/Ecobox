using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;

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

    private void Update()
    {
        if (m_ButtonPressed)
        {
            StopCoroutine(End());
            SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
        }
    }

    IEnumerator End()
    {
        yield return new WaitForSeconds(time);
        fade.SetTrigger("Start");
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }
}
