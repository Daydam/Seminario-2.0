using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class ChargeWeapon : Weapon
{
    public float maxChargeTime;

    public float bulletSpeed;
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

    protected override void InitializeUseCondition()
    {
        _canUseWeapon = () => !_owner.IsStunned && !_owner.IsDisarmed && !_owner.IsCasting;
    }

    protected override void CheckInput()
    {
        if (control.MainWeapon() && _canUseWeapon())
        {
            currentChargeTime = Mathf.Min(currentChargeTime + Time.deltaTime, maxChargeTime);
        }
        else if (currentChargeTime > 0)
        {
            Shoot();

            currentChargeTime = 0;
        }
    }

    public override void Shoot()
    {
        BulletSpawner.Instance.BulletPool.GetObjectFromPool().Spawn(bulletSpeed, damageByCharge.Evaluate(currentChargeTime), knockbackFalloff, transform.position, _owner.transform.rotation, gameObject.tag, _owner);
    }
}
*/