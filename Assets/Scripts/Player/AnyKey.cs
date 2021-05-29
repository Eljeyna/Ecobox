using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;

public class AnyKey : MonoBehaviour
{
    public bool m_ButtonPressed;
    
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
                    {
                        m_ButtonPressed = false;
                        continue;
                    }
                    
                    if (control.ReadValueFromEvent(eventPtr, out var value) && value >= buttonPressPoint)
                    {
                        m_ButtonPressed = true;
                        break;
                    }
                    
                    m_ButtonPressed = false;
                }
            };
    }
}
