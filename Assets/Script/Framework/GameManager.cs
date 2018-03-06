using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager: MonoBehaviour
{
	private static GameManager instance;
	public static GameManager Instance
	{
		get
		{
			if(instance == null)
			{
				instance = FindObjectOfType<GameManager>();
				if(instance == null)
				{
					instance = new GameObject("new GameManager Object").AddComponent<GameManager>().GetComponent<GameManager>();
				}
			}
			return instance;
		}
	}

    List<Player> players;
    public List<Player> Players { get { return players; } }


    public int Register(Player player)
    {
        if (players == null) players = new List<Player>();
        players.Add(player);
        return players.Count - 1;
    }
}
