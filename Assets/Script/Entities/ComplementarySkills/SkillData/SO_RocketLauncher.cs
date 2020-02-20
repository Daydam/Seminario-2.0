using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[CreateAssetMenu(fileName = "DATA_RocketLauncher", menuName = "Scriptable Objects/Skills/Complementary/RocketLauncher")]
public class SO_RocketLauncher : ScriptableObject
{
    public float maxCooldown;
    public float minRange, maxRange;
    public float maximumRadius;
    public float damage, knockback, speed;
}
