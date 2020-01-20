using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "DATA_WeaponBase", menuName = "Scriptable Objects/Weapons/WeaponBase")]
public class SO_WeaponBase : ScriptableObject
{
    [Header("Rounds per minute")]
    [Range(1, 10)]
    public int RPMScore;

    [Header("Damage")]
    public float minDamage;
    public float maxDamage;

    [Header("Falloff")]
    public float falloffStart;
    public float falloffEnd;

    [Header("KnockBack")]
    public float minKnockback;
    public float maxKnockback;

}
