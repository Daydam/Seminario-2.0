using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "DATA_StunMissile", menuName = "Scriptable Objects/Skills/Complementary/StunMissile")]
public class SO_StunMissile : ScriptableObject
{
    public float maxCooldown;
    public float minRange, maxRange;
    public float maximumRadius;
    public float damage, knockback, speed, duration;
}
