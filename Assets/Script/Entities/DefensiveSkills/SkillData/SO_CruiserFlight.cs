using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "DATA_CruiserFlight", menuName = "Scriptable Objects/Skills/Defensive/CruiserFlight")]
public class SO_CruiserFlight : ScriptableObject
{
    public float maxCooldown, airTime, stunPostLanding, fallTime, height, castTime;
    [Range(0, 1)]
    public float speedMultiplier;

    public float repulsiveForce, radius;
}
