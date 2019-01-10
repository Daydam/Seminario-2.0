using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "DATA_Vortex", menuName = "Scriptable Objects/Skills/Defensive/Vortex")]
public class SO_Vortex : ScriptableObject
{
    public float maxCooldown;
    public float blinkDistance;
    public float blinkDuration;
    public float disableDuration;
}
