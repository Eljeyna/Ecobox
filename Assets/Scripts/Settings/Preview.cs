using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class Preview : MonoBehaviour
{
    public string level;
    public float time = 5f;

    private bool buttonPressed;

    private void Start()
    {
        SceneLoading.Instance.PreloadLevel(level);
        StartCoroutine(End());
    }

    private void Update()
    {
        if (buttonPressed)
        {
            this.enabled = false;
            StopCoroutine(End());
            SceneLoading.Instance.SwitchToScene(level, SceneLoading.Instance.startAnimationID, true);
            return;
        }

        if (Game.GetInput())
        {
            buttonPressed = true;
        }
    }

    private IEnumerator End()
    {
        yield return new WaitForSecondsRealtime(time);

        if (!buttonPressed)
        {
            buttonPressed = true;
        }
    }

    public void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    public void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }
}
