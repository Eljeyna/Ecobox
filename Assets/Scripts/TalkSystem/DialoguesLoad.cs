using UnityEngine;

public class DialoguesLoad : MonoBehaviour
{
    public void Initialize()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent(out IsTalking isTalking))
            {
                isTalking.enabled = true;
            }
        }
    }
}
