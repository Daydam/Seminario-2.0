using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticWeapon : Weapon
{
    public float bulletSpeed;
    public float bulletDamage;
    public float maxCooldown;
    float currentCooldown = 0;

    protected override void CheckInput()
    {
        if (currentCooldown > 0) currentCooldown -= Time.deltaTime;
        else if(control.MainWeapon())
        {
            BulletSpawner.Instance.BulletPool.GetObjectFromPool().ConfigurateBullet(bulletSpeed, bulletDamage, transform.position, transform.rotation, gameObject.layer);
            currentCooldown = maxCooldown;
        }
    }
}
