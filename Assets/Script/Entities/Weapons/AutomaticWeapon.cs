using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticWeapon : Weapon
{
    protected override void CheckInput()
    {
        if (currentCooldown > 0) currentCooldown -= Time.deltaTime;
        else if(control.MainWeapon() && !_owner.IsStunned && !_owner.IsDisarmed)
        {
            new HitscanBullet(Owner.transform.position, Owner.transform.forward, Owner, damageFalloff, knockbackFalloff);

            //BulletSpawner.Instance.BulletPool.GetObjectFromPool().Spawn(bulletSpeed, damageFalloff, knockbackFalloff, transform.position, _owner.transform.rotation, gameObject.tag, _owner);
            currentCooldown = realCooldown;
        }
    }
}
