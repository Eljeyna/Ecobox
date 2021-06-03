using System;
using UnityEngine;

public class EventExit : MonoBehaviour
{
    public event EventHandler OnExit;

    public void Call()
    {
        OnExit?.Invoke(this, EventArgs.Empty);
    }
}
