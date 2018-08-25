using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Registered Players", menuName = "ScriptableObjects/Save Files/Registered Players")]
public class RegisteredPlayers : ScriptableObject
{
    public int[] playerControllers;
}
