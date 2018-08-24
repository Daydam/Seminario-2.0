using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapWeapon : Weapon
{
    bool canShoot = true;

    public override void Shoot()
    {
        new HitscanBullet(Owner.transform.position, Owner.transform.forward, Owner, damageFalloff, knockbackFalloff);
    }

    protected override void CheckInput()
    {
        if (currentCooldown > 0) currentCooldown -= Time.deltaTime;

        if (control.MainWeapon() && !_owner.IsStunned && !_owner.IsDisarmed)
        {
            if(canShoot && currentCooldown <= 0)
            {
                Shoot();

                //BulletSpawner.Instance.BulletPool.GetObjectFromPool().Spawn(bulletSpeed, damageFalloff, knockbackFalloff, transform.position, _owner.transform.rotation, gameObject.tag, _owner);
                canShoot = false;
                currentCooldown = realCooldown;
            }
        }
        else canShoot = true;
    }
}
