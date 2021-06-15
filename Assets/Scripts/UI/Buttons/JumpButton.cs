using UnityEngine;

public class JumpButton : MonoBehaviour
{
    public void Use()
    {
        Player.Instance.OnJump();
    }
}
