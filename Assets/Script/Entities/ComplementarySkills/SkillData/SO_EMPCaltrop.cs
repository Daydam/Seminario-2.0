using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[CreateAssetMenu(fileName = "DATA_EMPCaltrop", menuName = "Scriptable Objects/Skills/Complementary/EMPCaltrop")]
public class SO_EMPCaltrop : ScriptableObject
{
    public float maxCooldown;
    public byte maxChargesActive;
    public float duration, radius, amount;
}
