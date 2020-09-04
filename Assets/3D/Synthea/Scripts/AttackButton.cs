using TMPro;
using UnityEngine;

public class AttackButton : MonoBehaviour
{
    public TMP_Text text;
    private PlayerController thisPlayer;

    private void Start()
    {
        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            text.enabled = true;
            Destroy(gameObject);
        }
        else
        {
            thisPlayer = GameDirector3D.GetPlayer().GetComponent<PlayerController>();
        }
    }

    public void Attack()
    {
        thisPlayer.Attack();
    }
}
