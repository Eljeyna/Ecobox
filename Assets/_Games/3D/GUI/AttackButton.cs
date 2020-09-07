using UnityEngine;

public class AttackButton : MonoBehaviour
{
    private PlayerController thisPlayer;

    private void Start()
    {
#if UNITY_STANDALONE
        Destroy(gameObject);
#else
        thisPlayer = GameDirector3D.GetPlayer().GetComponent<PlayerController>();
#endif
    }

#if UNITY_ANDROID || UNITY_IOS
    public void Attack()
    {
        thisPlayer.Attack();
    }
#endif
}
