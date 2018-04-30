using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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

    RegisteredPlayers playerInfo;

    void Start()
    {
        var spawns = GameObject.Find("Stage").transform.Find("SpawnPoints").GetComponentsInChildren<Transform>().Where(x => x.name != "SpawnPoints").ToArray();

        playerInfo = Serializacion.LoadDataFromDisk<RegisteredPlayers>("Assets/Resources/Save Files/Registered Players.dat");

        for (int i = 0; i < playerInfo.playerControllers.Length; i++)
        {
            string path = "Assets/Resources/Save Files/Player " + playerInfo.playerControllers[i] + ".dat";
            var URLs = Serializacion.LoadDataFromDisk<CharacterURLs>(path);

            var player = Instantiate(Resources.Load<GameObject>(URLs.bodyURL), spawns[i].transform.position, Quaternion.identity).GetComponent<Player>();
            Instantiate(Resources.Load<GameObject>(URLs.weaponURL), player.transform.position, Quaternion.identity, player.transform);
            var comp1 = Instantiate(Resources.Load<GameObject>(URLs.complementaryURL[0]), player.transform.position, Quaternion.identity, player.transform);
            var comp2 = Instantiate(Resources.Load<GameObject>(URLs.complementaryURL[1]), player.transform.position, Quaternion.identity, player.transform);
            Instantiate(Resources.Load<GameObject>(URLs.defensiveURL), player.transform.position, Quaternion.identity, player.transform);

            comp1.GetComponent<ComplementarySkillBase>().RegisterInput(0);
            comp2.GetComponent<ComplementarySkillBase>().RegisterInput(1);

            player.gameObject.layer = LayerMask.NameToLayer("Player" + playerInfo.playerControllers[i]);
            player.gameObject.tag = "Player " + playerInfo.playerControllers[i];
            foreach (Transform t in player.transform)
            {
                t.gameObject.layer = LayerMask.NameToLayer("Player" + playerInfo.playerControllers[i]);
                t.gameObject.tag = "Player " + playerInfo.playerControllers[i];
            }
        }
    }

    public int Register(Player player)
    {
        if (players == null) players = new List<Player>();
        players.Add(player);
        return playerInfo.playerControllers[players.Count - 1];
    }

    public void Unregister(Player player)
    {
        players.Remove(player);
    }
}