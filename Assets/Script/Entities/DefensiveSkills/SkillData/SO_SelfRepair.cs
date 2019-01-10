using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "DATA_SelfRepair", menuName = "Scriptable Objects/Skills/Defensive/SelfRepair")]
public class SO_SelfRepair : ScriptableObject
{
    public float knockbackAdded;
    public float maxCharge;
    public float rechargePerSecond;
    public float ticks;
    public float repairPerSecond;
}
