using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    public int score = 0;
    public int kills = 0;
    public int deaths = 0;
    public int drops = 0;
    public float damageDealt = 0;
    public float damageTaken = 0;
    public int survived = 0;
    public int Score { get { return score; } set { score = value <= 0 ? 0 : value; } }
    public int Kills { get { return kills; } set { kills = value <= 0 ? 0 : value; } }
    public int Deaths { get { return deaths; } set { deaths = value <= 0 ? 0 : value; } }
    public int Drops { get { return drops; } set { drops = value <= 0 ? 0 : value; } }
    public float DamageDealt { get { return damageDealt; } set { damageDealt = value <= 0 ? 0 : value; } }
    public float DamageTaken { get { return damageTaken; } set { damageTaken = value <= 0 ? 0 : value; } }
    public int Survived { get { return survived; } set { survived = value <= 0 ? 0 : value; } }
}
