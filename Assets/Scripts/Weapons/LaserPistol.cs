using UnityEngine;

public class LaserPistol : Gun
{
    public BaseEntity thisEntity;
    public BaseTag baseTag;
    public Transform laserStartPosition;

    private void Update()
    {
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

        BulletSetup(Pool.Instance.GetFromPoolAsync(0).Result);

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
        {
            return false;
        }

        nextAttack = Time.time + gunData.reloadTime;
        reloading = true;

        return true;
    }

    public void BulletSetup(GameObject bullet)
    {
        if (bullet.TryGetComponent(out Bullet bulletPistol))
        {
            bulletPistol.owner = thisEntity;
            bulletPistol.baseTag = baseTag.entityTag;
            bullet.transform.position = laserStartPosition.position;
            bullet.transform.rotation = transform.rotation;
        }
    }
}
