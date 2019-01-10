using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "DATA_ImplosiveCharge", menuName = "Scriptable Objects/Skills/Complementary/ImplosiveCharge")]
public class SO_ImplosiveCharge : ScriptableObject
{
    public float maxCooldown, playerTravelTime;
    public float radius, speed, maxDistance;
}
