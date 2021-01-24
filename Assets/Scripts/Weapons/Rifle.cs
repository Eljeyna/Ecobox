using UnityEngine;

public class Rifle : Gun
{
    public BaseEntity thisEntity;
    public LineRenderer line;
    public Transform lineStartPosition;
    public float lineFade = 0.1f;

    [HideInInspector] public float nextFade;
    /*public float fadeSpeed = 5f;
    [HideInInspector] public float timeElapsed;

    private GradientAlphaKey[] standartGradientAlpha;

    private void Start()
    {
        standartGradientAlpha = line.colorGradient.alphaKeys;
    }*/

    private void Update()
    {
        if (line.isVisible)
        {
            if (nextFade <= Time.time)
            {
                line.enabled = false;

                /*timeElapsed = 0f;

                Gradient lineRendererGradient = new Gradient();
                lineRendererGradient.SetKeys
                (
                    line.colorGradient.colorKeys,
                    standartGradientAlpha
                );
                line.colorGradient = lineRendererGradient;*/
            }
            /*else
            {
                float alpha = Mathf.Lerp(0.5f, 0f, timeElapsed * fadeSpeed);

                Gradient lineRendererGradient = new Gradient();
                lineRendererGradient.SetKeys
                (
                    line.colorGradient.colorKeys,
                    new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0f) }
                );
                line.colorGradient = lineRendererGradient;

                timeElapsed += Time.deltaTime;
            }*/
        }

        if (reloading && nextAttack <= Time.time)
        {
            int cl = Mathf.Min(gunData.maxClip - clip, ammo);
            clip += cl;
            ammo -= cl;
            fireWhenEmpty = false;
            reloading = false;
        }

        if (clip != -1 && clip == 0 && nextAttack <= Time.time)
        {
            fireWhenEmpty = false;

            if (gunData.autoreload)
            {
                Reload();
            }
        }
    }

    public override void PrimaryAttack()
    {
        if (clip == 0 && fireWhenEmpty)
        {
            nextAttack = Time.time + 0.1f;
            return;
        }

        if (clip != -1)
        {
            clip--;
        }

        RaycastHit2D hit = Physics2D.Raycast(lineStartPosition.position, transform.up, gunData.range);
        line.SetPosition(0, lineStartPosition.position);

        if (hit)
        {
            if (Vector2.Distance(lineStartPosition.position, hit.point) > 1f)
            {
                nextFade = Time.time + lineFade;
                line.SetPosition(1, hit.point * 1.1f);
                line.enabled = true;

            }
            BaseEntity entity = hit.collider.GetComponent<BaseEntity>();
            if (entity != null)
            {
                entity.TakeDamage(gunData.damage, 0, thisEntity);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * gunData.impactForce, ForceMode2D.Impulse);
            }
        }
        else
        {
            nextFade = Time.time + lineFade;
            line.SetPosition(1, transform.position + transform.up * gunData.range);
            line.enabled = true;
        }

        nextAttack = Time.time + gunData.fireRatePrimary;
    }

    public override void SecondaryAttack()
    {
        return;
    }

    public override bool Reload()
    {
        if (ammo <= 0)
        {
            return false;
        }

        int cl = Mathf.Min(gunData.maxClip - clip, ammo);

        if (cl <= 0)
            return false;

        nextAttack = Time.time + gunData.reloadTime;
        reloading = true;

        return true;
    }
}
