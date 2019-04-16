using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "DATA_Hook", menuName = "Scriptable Objects/Skills/Complementary/Hook")]
public class SO_Hook : ScriptableObject
{
    public float playerTravelTime;
    public float stunIfNullTarget;
    public float castTime;
    public float maxCooldown;
    public float maxRange, travelTime, latchDelay;
}
