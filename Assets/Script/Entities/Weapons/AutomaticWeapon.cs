using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticWeapon : Weapon
{
    public float maxCooldown;
    float currentCooldown = 0;

    protected override void CheckInput()
    {
        if (currentCooldown > 0) currentCooldown -= Time.deltaTime;
        else if(control.MainWeapon())
        {
            print("shoot");
            currentCooldown = maxCooldown;
        }
    }
}
