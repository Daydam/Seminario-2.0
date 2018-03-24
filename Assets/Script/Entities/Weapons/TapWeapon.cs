using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapWeapon : Weapon
{
    public float bulletSpeed;
    public float bulletDamage;
    bool canShoot = true;

    protected override void CheckInput()
    {
        if (control.MainWeapon())
        {
            if(canShoot)
            {
                BulletSpawner.Instance.BulletPool.GetObjectFromPool().ConfigurateBullet(bulletSpeed, bulletDamage, transform.position, transform.rotation, gameObject.layer);
                canShoot = false;
            }
        }
        else canShoot = true;
    }
}
