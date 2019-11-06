using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[CreateAssetMenu(fileName = "DATA_WeaponCooldowns", menuName = "Scriptable Objects/Weapons/WeaponCooldowns")]
public class SO_WeaponCooldowns : ScriptableObject
{
    [Header("Multiplier for all weapon cooldowns")]
    public float cooldownMultiplier = 1;

    [SerializeField]
    public float[] cooldownValues = new float[10]
    {
        1,
        0.5217391304f,
        0.3692307692f,
        0.3f,
        0.2307692308f,
        0.2f,0.1846153846f,
        0.16f,0.133333333f,
        0.1f
    };

}
