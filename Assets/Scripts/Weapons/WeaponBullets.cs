using UnityEngine;

public class WeaponBullets : Gun
{
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
        
        StatePerform();
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

    public override async void Attack()
    {
        if (clip != -1)
        {
            clip--;
        }

        BulletSetup(await Pool.Instance.GetFromPoolAsync((int)PoolID.SimpleBullet));
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
            bulletPistol.owner = entity.thisEntity;
            bulletPistol.baseTag = entity.thisTag;
            bullet.transform.position = attackPoint.position;
            bullet.transform.rotation = Quaternion.AngleAxis(
                StaticGameVariables.GetAngleBetweenPositions(entity.targetPosition, entity.transform.position), Vector3.forward);
        }
    }
}
