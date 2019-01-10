using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "DATA_PlasmaWall", menuName = "Scriptable Objects/Skills/Defensive/PlasmaWall")]
public class SO_PlasmaWall : ScriptableObject
{
    public float maxCooldown;

    public float maxHP, lifeTime, size;
}
