using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "DATA_Dash", menuName = "Scriptable Objects/Skills/Defensive/Dash")]
public class SO_Dash : ScriptableObject
{
    public float maxCooldown;
    public float dashDistance;
    public float dashTime;
}

