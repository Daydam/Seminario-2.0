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

    #region Cambios Iván 3/4/18
    /// <summary>
    /// Es para randomizar armas por ahora, cuando esté la ventana de selección esto vuela
    /// </summary>
    List<Weapon> weapons;
    /// <summary>
    /// Es para randomizar armas por ahora, cuando esté la ventana de selección esto vuela
    /// </summary>
    public List<Weapon> AllWeapons { get { return weapons; } }
    #endregion

    RegisteredPlayers playerInfo;

    void Start()
    {
        var spawns = GameObject.Find("Stage").transform.Find("SpawnPoints").GetComponentsInChildren<Transform>().Where(x => x.name != "SpawnPoints").ToArray();

        //esto es por ahora, hasta tener hechas las configuraciones de cantidad de jugadores y spawn points.
        //Instantiate(Resources.Load<GameObject>("Prefabs/Player 1"), spawns[0].transform.position, Quaternion.identity);
        //Instantiate(Resources.Load<GameObject>("Prefabs/Player 2"), spawns[1].transform.position, Quaternion.identity);
        //Instantiate(Resources.Load<GameObject>("Prefabs/Player 3"), spawns[2].transform.position, Quaternion.identity);
        //Instantiate(Resources.Load<GameObject>("Prefabs/Player 4"), spawns[3].transform.position, Quaternion.identity);

        playerInfo = Serializacion.LoadDataFromDisk<RegisteredPlayers>("Assets/Resources/Save Files/Registered Players.dat");

        for (int i = 0; i < playerInfo.playerControllers.Length; i++)
        {
            string path = "Assets/Resources/Save Files/Player " + playerInfo.playerControllers[i] + ".dat";
            var URLs = Serializacion.LoadDataFromDisk<CharacterURLs>(path);
            if (URLs == default(CharacterURLs))
            {
                URLs = new CharacterURLs
                {
                    bodyURL = "Prefabs/Bodies/Body " + playerInfo.playerControllers[i],
                    weaponURL = "Prefabs/Weapons/Assault Rifle"
                };
            }

            var player = Instantiate(Resources.Load<GameObject>(URLs.bodyURL), spawns[i].transform.position, Quaternion.identity).GetComponent<Player>();
            Instantiate(Resources.Load<GameObject>(URLs.weaponURL), player.transform.position, Quaternion.identity, player.transform).GetComponent<Player>();

            player.gameObject.layer = LayerMask.NameToLayer("Player" + playerInfo.playerControllers[i]);
            player.gameObject.tag = "Player " + playerInfo.playerControllers[i];
            foreach (Transform t in player.transform)
            {
                t.gameObject.layer = LayerMask.NameToLayer("Player" + playerInfo.playerControllers[i]);
                t.gameObject.tag = "Player " + playerInfo.playerControllers[i];
            }
        }

        /*weapons = new List<Weapon>();
        var filterList = Resources.LoadAll("Prefabs/Weapons").Select(x => (GameObject)x);
        weapons = filterList.Select(x => x.GetComponent<Weapon>()).ToList();*/
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
