using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Weapon : MonoBehaviour
{
    //CREATE A FEW CLASSES THAT INHERIT FROM THIS, AND MAKE THIS AN INTERFACE. CREATE "TapWeapon" and "AutomaticWeapon".
    //In each case, create cooldown timers to make sure no dumbass can spam bullets.

    protected Controller control;
    protected Player _me;
    public AnimationCurve damageFalloff;

    [Range(1,10)]
    public int maxCooldown;

    protected float realCooldown;
    protected float currentCooldown = 0;

    static Dictionary<int, float> weaponRealCooldowns;
    public static Dictionary<int, float> WeaponCooldowns { get { return weaponRealCooldowns; } }

    public float bulletSpeed;
    public float minDamage;
    public float maxDamage;
    public float falloffStart;
    public float falloffEnd;

    protected virtual void Awake()
    {
        InitializeCooldowns();
        SetCurveValues();
    }

    protected virtual void SetCurveValues()
    {
        damageFalloff = new AnimationCurve();
        var initialKey = new Keyframe(0, maxDamage, 0, 0);
        damageFalloff.AddKey(initialKey);
        var startFalloff = new Keyframe(falloffStart, maxDamage, 0, 0);
        damageFalloff.AddKey(startFalloff);
        var endtFalloff = new Keyframe(falloffEnd, minDamage, 0, 0);
        damageFalloff.AddKey(endtFalloff);
    }

    void Start()
    {
        realCooldown = WeaponCooldowns[maxCooldown];
        _me = GetComponentInParent<Player>();
        control = _me.Control;
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
