using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "DATA_RepulsiveBattery", menuName = "Scriptable Objects/Skills/Defensive/RepulsiveBattery")]
public class SO_RepulsiveBattery : ScriptableObject
{
    public float maxCooldown, castTime;

    public float repulsiveForce, radius, shieldDuration;
}
