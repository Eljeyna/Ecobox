using UnityEngine;

public class ButtonAttack : MonoBehaviour
{
    public void Use()
    {
        Player.Instance.touch = false;

        if (Player.Instance.state != EntityState.Stun)
        {
            Player.Instance.Attack();
        }
    }
}
