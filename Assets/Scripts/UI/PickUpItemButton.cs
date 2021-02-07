using UnityEngine;

public class PickUpItemButton : MonoBehaviour
{
    public void Use()
    {
        Player.Instance.PickUpItem();
    }
}
