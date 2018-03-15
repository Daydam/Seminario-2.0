using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    instance = new GameObject("new GameManager Object").AddComponent<GameManager>().GetComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    List<Player> players;
    public List<Player> Players { get { return players; } }

    void Start()
    {
        //esto es por ahora, hasta tener hechas las configuraciones de cantidad de jugadores y spawn points.
        Instantiate(Resources.Load<GameObject>("Prefabs/Player 1"), new Vector3(0, 1.5f, 30), Quaternion.identity);
        Instantiate(Resources.Load<GameObject>("Prefabs/Player 2"), new Vector3(0, 1.5f, -30), Quaternion.identity);
        Instantiate(Resources.Load<GameObject>("Prefabs/Player 3"), new Vector3(-30, 1.5f, 0), Quaternion.identity);
        Instantiate(Resources.Load<GameObject>("Prefabs/Player 4"), new Vector3(30, 1.5f, 0), Quaternion.identity);
    }

    public int Register(Player player)
    {
        if (players == null) players = new List<Player>();
        players.Add(player);
        return players.Count - 1;
    }
}
