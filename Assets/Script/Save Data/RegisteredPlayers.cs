using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RegisteredPlayers
{
    public int[] playerControllers;
    public PlayerStats[] playerStats;
    public string gameMode = "Standard";
}
