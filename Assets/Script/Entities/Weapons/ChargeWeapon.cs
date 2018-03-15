using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeWeapon : Weapon
{
    public float maxChargeTime;
    float currentChargeTime;

    protected override void CheckInput()
    {
        if (control.MainWeapon())
        {
            currentChargeTime = Mathf.Min(currentChargeTime + Time.deltaTime, maxChargeTime);
        }
        else if (currentChargeTime > 0)
        {
            print("KABOOM " + currentChargeTime);
            currentChargeTime = 0;
        }
    }
}
