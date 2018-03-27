using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Weapon : MonoBehaviour
{
    //CREATE A FEW CLASSES THAT INHERIT FROM THIS, AND MAKE THIS AN INTERFACE. CREATE "TapWeapon" and "AutomaticWeapon".
    //In each case, create cooldown timers to make sure no dumbass can spam bullets.

    protected Controller control;
    public AnimationCurve damageFalloff;

    public float minDamage;
    public float maxDamage;
    public float falloffStart;
    public float falloffEnd;

    void Start()
    {
        control = GetComponentInParent<Player>().Control;
    }

    void Update()
    {
        CheckInput();
    }

    protected abstract void CheckInput();
}
