using UnityEngine;

public class JumpButton : MonoBehaviour
{
    public void Use()
    {
        Player.Instance.touch = false;
        Player.Instance.OnJump();
    }
}
