using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Weapon : MonoBehaviour
{
    string _weaponCooldownPath = "Scriptable Objects/Weapons/DATA_WeaponCooldowns";
    public SO_WeaponBase weaponStatsData;
    protected SO_WeaponCooldowns _weaponCooldownData;

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

    protected Transform _muzzle;

    protected float VibrationDuration
    {
        get { return Mathf.Min(_realCooldown, .2f); }
    }

    protected float VibrationIntensity
    {
        get { return Mathf.Min(weaponStatsData.maxDamage / 60, 1.5f); }
    }

    protected float ShakeDuration
    {
        get { return Mathf.Min(_realCooldown, .2f); }
    }

    protected float ShakeIntensity
    {
        get { return Mathf.Max(weaponStatsData.maxDamage / 50, 2); }
    }

    protected virtual void Awake()
    {
        _weaponCooldownData = ScriptableObject.Instantiate(Resources.Load<SO_WeaponCooldowns>(_weaponCooldownPath) as SO_WeaponCooldowns);

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
        var initialKey = new Keyframe(0, weaponStatsData.maxDamage, 0, 0);
        _damageFalloff.AddKey(initialKey);
        var startFalloff = new Keyframe(weaponStatsData.falloffStart, weaponStatsData.maxDamage, 0, 0);
        _damageFalloff.AddKey(startFalloff);
        var endFalloff = new Keyframe(weaponStatsData.falloffEnd, weaponStatsData.minDamage, 0, 0);
        _damageFalloff.AddKey(endFalloff);
    }

    void SetKnockbackCurve()
    {
        _knockbackFalloff = new AnimationCurve();
        var initialKey = new Keyframe(0, weaponStatsData.maxKnockback, 0, 0);
        _knockbackFalloff.AddKey(initialKey);
        var startFalloff = new Keyframe(weaponStatsData.falloffStart, weaponStatsData.maxKnockback, 0, 0);
        _knockbackFalloff.AddKey(startFalloff);
        var endFalloff = new Keyframe(weaponStatsData.falloffEnd, weaponStatsData.minKnockback, 0, 0);
        _knockbackFalloff.AddKey(endFalloff);
    }

    void Start()
    {
        InitializeUseCondition();
        _realCooldown = WeaponCooldowns[weaponStatsData.RPMScore];
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

        for (int i = 0; i < _weaponCooldownData.cooldownValues.Length; i++)
        {
            weaponRealCooldowns.Add(i + 1, _weaponCooldownData.cooldownValues[i]);
        }
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
