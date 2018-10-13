using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Weapon : MonoBehaviour
{
    protected Controller control;
    protected Player _owner;
    /*[SerializeField]*/
    protected AnimationCurve damageFalloff;
    /*[SerializeField]*/
    protected AnimationCurve knockbackFalloff;
    protected Func<bool> _canUseWeapon;

    [Range(1, 10)]
    public int RPMScore;

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

    public float minDamage;
    public float maxDamage;
    public float minKnockback = 2.5f;
    public float maxKnockback = 7.5f;
    public float falloffStart;
    public float falloffEnd;

    protected Transform _muzzle;

    protected float VibrationDuration
    {
        get { return Mathf.Min(realCooldown, .2f); }
    }

    protected float VibrationIntensity
    {
        get { return Mathf.Min(maxDamage / 40, 1.5f); }
    }

    protected float ShakeDuration
    {
        get { return Mathf.Min(realCooldown, .2f); }
    }

    protected float ShakeIntensity
    {
        get { return Mathf.Max(maxDamage / 50, 2); }
    }

    protected virtual void Awake()
    {
        InitializeCooldowns(1);
        SetCurveValues();
        _muzzle = transform.Find("Muzzle");
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
        realCooldown = WeaponCooldowns[RPMScore];
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
    void InitializeCooldowns(float multiplier = 1)
    {
        weaponRealCooldowns = new Dictionary<int, float>();

        weaponRealCooldowns.Add(1, 1 / multiplier);
        weaponRealCooldowns.Add(2, 0.5217391304f / multiplier);
        weaponRealCooldowns.Add(3, 0.3692307692f / multiplier);
        weaponRealCooldowns.Add(4, 0.3f / multiplier);
        weaponRealCooldowns.Add(5, 0.2307692308f / multiplier);
        weaponRealCooldowns.Add(6, 0.2f / multiplier);
        weaponRealCooldowns.Add(7, 0.1846153846f / multiplier);
        weaponRealCooldowns.Add(8, 0.16f / multiplier);
        weaponRealCooldowns.Add(9, 0.133333333f / multiplier);
        weaponRealCooldowns.Add(10, 0.1f / multiplier);
    }

    protected abstract void InitializeUseCondition();

    protected abstract void CheckInput();

    public abstract void Shoot();
}
