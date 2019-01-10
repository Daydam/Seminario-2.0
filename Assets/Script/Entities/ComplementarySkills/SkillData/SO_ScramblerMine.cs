using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using System.Runtime.Serialization;

[CreateAssetMenu(fileName = "DATA_ScramblerMine", menuName = "Scriptable Objects/Skills/Complementary/ScramblerMine")]
public class SO_ScramblerMine : ScriptableObject
{
    public float maxCooldown, duration, radius;
    public float damage, activationDelay, speed, maxHP;
}
