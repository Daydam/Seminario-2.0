using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticWeapon : Weapon
{
    protected override void CheckInput()
    {
        if (currentCooldown > 0) currentCooldown -= Time.deltaTime;
        else if(control.MainWeapon())
        {
            BulletSpawner.Instance.BulletPool.GetObjectFromPool().ConfigurateBullet(bulletSpeed, damageFalloff, transform.position, transform.rotation, gameObject.tag);
            currentCooldown = realCooldown;
        }
    }
}
