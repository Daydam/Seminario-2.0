using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeWeapon : Weapon
{
    public float maxChargeTime;

    float currentChargeTime;
    public AnimationCurve damageByCharge;

    protected override void Awake()
    {
        base.Awake();
        SetChargeCurveValues();
    }

    void SetChargeCurveValues()
    {
        damageByCharge = new AnimationCurve();
        var minChargeKey = new Keyframe(0, minDamage, 0, 0);
        damageByCharge.AddKey(minChargeKey);
        var maxChargeKey = new Keyframe(maxChargeTime, maxDamage, 0, 0);
        damageByCharge.AddKey(maxChargeKey);
    }

    protected override void CheckInput()
    {
        if (control.MainWeapon())
        {
            currentChargeTime = Mathf.Min(currentChargeTime + Time.deltaTime, maxChargeTime);
        }
        else if (currentChargeTime > 0)
        {
            BulletSpawner.Instance.BulletPool.GetObjectFromPool().ConfigurateBullet(bulletSpeed, damageFalloff, transform.position, transform.rotation, gameObject.tag);
            currentChargeTime = 0;
        }
    }
}
