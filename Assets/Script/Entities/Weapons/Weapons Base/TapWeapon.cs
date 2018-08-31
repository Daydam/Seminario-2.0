using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapWeapon : Weapon
{
    bool canShoot = true;

    public override void Shoot()
    {
        Owner.ApplyVibration(0, VibrationIntensity, VibrationDuration);
        Owner.ApplyShake(ShakeDuration, ShakeIntensity);
        new HitscanBullet(Owner.transform.position, Owner.transform.forward, Owner, damageFalloff, knockbackFalloff, 1);
    }

    protected override void CheckInput()
    {
        if (currentCooldown > 0) currentCooldown -= Time.deltaTime;

        if (control.MainWeapon() && !_owner.IsStunned && !_owner.IsDisarmed)
        {
            if(canShoot && currentCooldown <= 0)
            {
                Shoot();

                canShoot = false;
                currentCooldown = realCooldown;
            }
        }
        else canShoot = true;
    }
}
