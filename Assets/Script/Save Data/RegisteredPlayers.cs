using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RegisteredPlayers
{
    public static int latestRegVersion = 101;
    public int fileRegVersion;
    public int[] playerControllers;
    public PlayerStats[] playerStats;
    public string gameMode;
    public int stage;

    public RegisteredPlayers()
    {
        fileRegVersion = latestRegVersion;
        gameMode = "Standard";
        stage = 3;
    }
}