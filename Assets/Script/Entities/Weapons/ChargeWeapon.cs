using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeWeapon : Weapon
{
    public float bulletSpeed;
    public float maxChargeTime;
    public float maxCooldown;
    float currentCooldown = 0;
    float currentChargeTime;
    public AnimationCurve damageByCharge;


    protected override void CheckInput()
    {
        if (control.MainWeapon())
        {
            currentChargeTime = Mathf.Min(currentChargeTime + Time.deltaTime, maxChargeTime);
        }
        else if (currentChargeTime > 0)
        {
            BulletSpawner.Instance.BulletPool.GetObjectFromPool().ConfigurateBullet(bulletSpeed, damageFalloff, transform.position, transform.rotation, gameObject.layer);
            currentChargeTime = 0;
        }
    }
}
