using UnityEngine;

public class DashButton : MonoBehaviour
{
    public void Use()
    {
        Player.Instance.OnDash();
    }
}
