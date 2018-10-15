﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Weapon : MonoBehaviour
{
    protected Controller _control;
    protected Player _owner;
    /*[SerializeField]*/
    protected AnimationCurve _damageFalloff;
    /*[SerializeField]*/
    protected AnimationCurve _knockbackFalloff;
    protected Func<bool> _canUseWeapon;

    public AudioClip shootSound;
    protected AudioSource _audioSource;
    protected float _audioSourceOriginalPitch;

    [Range(1, 10)]
    public int RPMScore;

    protected float _realCooldown;
    protected float _currentCooldown = 0;

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
        get { return Mathf.Min(_realCooldown, .2f); }
    }

    protected float VibrationIntensity
    {
        get { return Mathf.Min(maxDamage / 40, 1.5f); }
    }

    protected float ShakeDuration
    {
        get { return Mathf.Min(_realCooldown, .2f); }
    }

    protected float ShakeIntensity
    {
        get { return Mathf.Max(maxDamage / 50, 2); }
    }

    protected virtual void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSourceOriginalPitch = _audioSource.pitch;
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
        _damageFalloff = new AnimationCurve();
        var initialKey = new Keyframe(0, maxDamage, 0, 0);
        _damageFalloff.AddKey(initialKey);
        var startFalloff = new Keyframe(falloffStart, maxDamage, 0, 0);
        _damageFalloff.AddKey(startFalloff);
        var endFalloff = new Keyframe(falloffEnd, minDamage, 0, 0);
        _damageFalloff.AddKey(endFalloff);
    }

    void SetKnockbackCurve()
    {
        _knockbackFalloff = new AnimationCurve();
        var initialKey = new Keyframe(0, maxKnockback, 0, 0);
        _knockbackFalloff.AddKey(initialKey);
        var startFalloff = new Keyframe(falloffStart, maxKnockback, 0, 0);
        _knockbackFalloff.AddKey(startFalloff);
        var endFalloff = new Keyframe(falloffEnd, minKnockback, 0, 0);
        _knockbackFalloff.AddKey(endFalloff);
    }

    void Start()
    {
        InitializeUseCondition();
        _realCooldown = WeaponCooldowns[RPMScore];
        _owner = GetComponentInParent<Player>();
        _control = _owner.Control;
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

    public virtual void PlaySound(AudioClip sound)
    {
        _audioSource.PlayOneShot(sound);
    }

    [Obsolete("No usar ahora", true)]
    public virtual void PlaySound(AudioClip sound, float minPitch, float maxPitch)
    {
        _audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
        _audioSource.PlayOneShot(sound);
    }
}
