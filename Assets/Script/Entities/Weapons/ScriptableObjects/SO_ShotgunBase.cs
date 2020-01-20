using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[CreateAssetMenu(fileName = "DATA_ShotgunBase", menuName = "Scriptable Objects/Weapons/ShotgunBase")]
public class SO_ShotgunBase : SO_WeaponBase
{
    public float maxDispersionRadius = 0.1f;
    public int pellets = 12;
}
