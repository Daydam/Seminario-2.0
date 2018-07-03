using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//[ExecuteInEditMode]
public abstract class Weapon : MonoBehaviour
{
    //CREATE A FEW CLASSES THAT INHERIT FROM THIS, AND MAKE THIS AN INTERFACE. CREATE "TapWeapon" and "AutomaticWeapon".
    //In each case, create cooldown timers to make sure no dumbass can spam bullets.

    protected Controller control;
    protected Player _owner;
    public AnimationCurve damageFalloff;
    public AnimationCurve knockbackFalloff;

    //public bool IDIOTA = false;

    [Range(1, 10)]
    public int maxCooldown;

    protected float realCooldown;
    protected float currentCooldown = 0;

    static Dictionary<int, float> weaponRealCooldowns;
    public static Dictionary<int, float> WeaponCooldowns { get { return weaponRealCooldowns; } }

    public Player Owner
    {
        get
        {
            return _owner;
        }
    }

    public float bulletSpeed;
    public float minDamage;
    public float maxDamage;
    float minKnockback = 5;
    float maxKnockback = 15;
    public float falloffStart;
    public float falloffEnd;

    protected virtual void Awake()
    {
        InitializeCooldowns(2);
        SetCurveValues();
    }

    protected virtual void SetCurveValues()
    {
        SetDamageCurve();
        SetKnockbackCurve();
    }

    void SetDamageCurve()
    {
        damageFalloff = new AnimationCurve();
        var initialKey = new Keyframe(0, maxDamage, 0, 0);
        damageFalloff.AddKey(initialKey);
        var startFalloff = new Keyframe(falloffStart, maxDamage, 0, 0);
        damageFalloff.AddKey(startFalloff);
        var endFalloff = new Keyframe(falloffEnd, minDamage, 0, 0);
        damageFalloff.AddKey(endFalloff);
    }

    void SetKnockbackCurve()
    {
        knockbackFalloff = new AnimationCurve();
        var initialKey = new Keyframe(0, maxKnockback, 0, 0);
        knockbackFalloff.AddKey(initialKey);
        var startFalloff = new Keyframe(falloffStart, maxKnockback, 0, 0);
        knockbackFalloff.AddKey(startFalloff);
        var endFalloff = new Keyframe(falloffEnd, minKnockback, 0, 0);
        knockbackFalloff.AddKey(endFalloff);
    }

    void Start()
    {
        realCooldown = WeaponCooldowns[maxCooldown];
        _owner = GetComponentInParent<Player>();
        control = _owner.Control;
    }

    void Update()
    {
        SetKnockbackCurve();
        CheckInput();
    }

    /// <summary>
    /// TODO !!
    /// CARGAR DESDE ARCHIVO
    /// </summary>
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

    /// <summary>
    /// TODO !!
    /// CARGAR DESDE ARCHIVO
    /// </summary>
    void InitializeCooldowns(float multiplier)
    {
        weaponRealCooldowns = new Dictionary<int, float>();

        weaponRealCooldowns.Add(1, 1.24f / multiplier);
        weaponRealCooldowns.Add(2, 1.11f / multiplier);
        weaponRealCooldowns.Add(3, 0.98f / multiplier);
        weaponRealCooldowns.Add(4, 0.85f / multiplier);
        weaponRealCooldowns.Add(5, 0.72f / multiplier);
        weaponRealCooldowns.Add(6, 0.59f / multiplier);
        weaponRealCooldowns.Add(7, 0.46f / multiplier);
        weaponRealCooldowns.Add(8, 0.33f / multiplier);
        weaponRealCooldowns.Add(9, 0.2f / multiplier);
        weaponRealCooldowns.Add(10, 0.1f / multiplier);
    }

    protected abstract void CheckInput();
}
