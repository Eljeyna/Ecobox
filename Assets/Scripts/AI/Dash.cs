using UnityEngine;

public class Dash : MonoBehaviour
{
    public int staminaCost = 5;
    public float dashTime = 0.25f;
    public float dashReload = 0.5f;
    public AnimationCurve dashSpeed;
    public float dashEvaluateTime;

    [HideInInspector] public float nextDash;
    [HideInInspector] public float nextDashTime;

    public void Update()
    {
        if (nextDashTime > Time.time)
        {
            this.enabled = false;
        }
    }

    public void Use()
    {
        nextDash = Time.time + dashTime;
        nextDashTime = Time.time + dashReload;
        this.enabled = true;
    }
}
