using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DroneWeightModule : MonoBehaviour
{
    public int weight;

    [HideInInspector]
    public readonly string prefix = "DATA_";

    public string sufix;
}
