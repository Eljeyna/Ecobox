using UnityEngine;

public class DashButton : MonoBehaviour
{
    public void Use()
    {
        Player.Instance.touch = false;
        Player.Instance.OnDash();
    }
}
