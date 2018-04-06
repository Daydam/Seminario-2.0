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

    [Range(1, 10)]
    public int maxCooldown;

    protected float realCooldown;
    protected float currentCooldown = 0;

    static Dictionary<int, float> weaponRealCooldowns;
    public static Dictionary<int, float> WeaponCooldowns { get { return weaponRealCooldowns; } }

    public float minDamage;
    public float maxDamage;
    public float falloffStart;
    public float falloffEnd;

    void Start()
    {
        InitializeCooldowns();
        control = GetComponentInParent<Player>().Control;
        realCooldown = WeaponCooldowns[maxCooldown];
    }

    void Update()
    {
        CheckInput();
    }

    void InitializeCooldowns()
    {
        weaponRealCooldowns = new Dictionary<int, float>();

        weaponRealCooldowns.Add(1, 1.24f);
        weaponRealCooldowns.Add(2, 1.11f);
        weaponRealCooldowns.Add(3, 0.98f);
        weaponRealCooldowns.Add(4, 0.85f);
        weaponRealCooldowns.Add(5, 0.72f);
        weaponRealCooldowns.Add(6, 0.59f);
        weaponRealCooldowns.Add(7, 0.46f);
        weaponRealCooldowns.Add(8, 0.33f);
        weaponRealCooldowns.Add(9, 0.2f);
        weaponRealCooldowns.Add(10, 0.1f);
    }

    protected abstract void CheckInput();
}
