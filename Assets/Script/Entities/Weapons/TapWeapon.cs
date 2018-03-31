using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapWeapon : Weapon
{
    public float bulletSpeed;
    public float maxCooldown;
    float currentCooldown = 0;
    bool canShoot = true;

    protected override void CheckInput()
    {
        if (currentCooldown > 0) currentCooldown -= Time.deltaTime;

        if (control.MainWeapon())
        {
            if(canShoot && currentCooldown <= 0)
            {
                BulletSpawner.Instance.BulletPool.GetObjectFromPool().ConfigurateBullet(bulletSpeed, damageFalloff, transform.position, transform.rotation, gameObject.layer);
                canShoot = false;
                currentCooldown = maxCooldown;
            }
        }
        else canShoot = true;
    }
}
