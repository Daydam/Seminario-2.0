using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Game Rules", menuName = "Scriptable Objects/Configuration/Game Rules")]
public class SO_GameRules : ScriptableObject
{
    //Kill another player
    public int pointsPerKill;
    //Drop another player from the stage
    public int pointsPerDrop;
    //Suicide
    public int pointsPerSuicide;
    //Be the last drone flying (Get it? Instead of Last Man Standing? Yeah, I'm fun like that)
    public int pointsForLast;

    //Points required to win a match
    public int[] pointsToWin;
}