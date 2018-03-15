using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TapWeapon : Weapon
{
    bool canShoot = true;

    protected override void CheckInput()
    {
        if (control.MainWeapon())
        {
            if(canShoot)
            {
                Debug.Log("I just shat");
                canShoot = false;
            }
        }
        else canShoot = true;
    }
}
