using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "DATA_RocketSalvo", menuName = "Scriptable Objects/Skills/Complementary/RocketSalvo")]
public class SO_RocketSalvo : ScriptableObject
{
    public int rocketCount;
    public float maxCooldown, duration, effectRadius;
    public float explosionRadius, damage, speed;

    public float RocketCooldown { get { return duration / rocketCount; } }

}

