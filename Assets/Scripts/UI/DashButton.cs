using UnityEngine;

public class DashButton : MonoBehaviour
{
    public void Use()
    {
        Player.Instance.touch = false;

        if (Player.Instance.state != EntityState.Stun)
        {
            Player.Instance.OnDash();
        }
    }
}
