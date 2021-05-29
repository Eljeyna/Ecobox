using UnityEngine;

public class ActivateObjectWithTime : MonoBehaviour
{
    public GameObject objectForActivate;
    public float waitTime;
    public bool destroyOnExecute;

    private float nextWait;

    private void Awake()
    {
        nextWait = Time.unscaledTime + waitTime;
    }

    private void Update()
    {
        if (nextWait > Time.unscaledTime)
        {
            return;
        }

        objectForActivate.SetActive(true);
        this.enabled = false;

        if (destroyOnExecute)
        {
            Destroy(this);
        }
    }
}
