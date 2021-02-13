using UnityEngine;

public class Rifle : Gun
{
    public BaseEntity thisEntity;
    public LineRenderer line;
    public Transform lineStartPosition;
    public float lineFade = 0.1f;

    [HideInInspector] public float nextFade;

    private void Update()
    {
        StatePerform();

        if (line.isVisible)
        {
            if (nextFade <= Time.time)
            {
                line.enabled = false;
            }
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

        base.PrimaryAttack();
    }

    public override void Attack()
    {
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
            if (hit.collider.TryGetComponent(out BaseEntity entity))
            {
                entity?.TakeDamage(gunData.damage, 0, thisEntity);
            }

            hit.rigidbody?.AddForce(-hit.normal * gunData.impactForce, ForceMode2D.Impulse);
        }
        else
        {
            nextFade = Time.time + lineFade;
            line.SetPosition(1, transform.position + transform.up * gunData.range);
            line.enabled = true;
        }
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
