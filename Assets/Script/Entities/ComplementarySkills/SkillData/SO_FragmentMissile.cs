using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[CreateAssetMenu(fileName = "DATA_FragmentMissile", menuName = "Scriptable Objects/Skills/Complementary/FragmentMissile")]
public class SO_FragmentMissile : ScriptableObject
{
    public float maxCooldown;
    public float minRange, maxRange;
    public float maximumRadius;
    public float damage, knockback, speed;
}
