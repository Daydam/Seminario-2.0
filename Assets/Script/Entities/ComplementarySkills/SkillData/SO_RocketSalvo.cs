using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "DATA_RocketSalvo", menuName = "Scriptable Objects/Skills/Complementary/RocketSalvo")]
public class SO_RocketSalvo : ScriptableObject
{
    public int rocketCount;
    public float maxCooldown, duration, effectRadius, rocketCooldown;
    public float explosionRadius, damage, speed;
}

