using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapWeapon : Weapon
{
    bool canShoot = true;

    protected override void CheckInput()
    {
        if (currentCooldown > 0) currentCooldown -= Time.deltaTime;

        if (control.MainWeapon())
        {
            if(canShoot && currentCooldown <= 0)
            {
                BulletSpawner.Instance.BulletPool.GetObjectFromPool().ConfigurateBullet(bulletSpeed, damageFalloff, transform.position, transform.rotation, gameObject.tag);
                canShoot = false;
                currentCooldown = realCooldown;
            }
        }
        else canShoot = true;
    }
}
