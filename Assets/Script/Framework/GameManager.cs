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

    void Start()
    {
        var spawns = GameObject.Find("Stage").transform.Find("SpawnPoints").GetComponentsInChildren<Transform>().Where(x => x.name != "SpawnPoints").ToArray();

        //esto es por ahora, hasta tener hechas las configuraciones de cantidad de jugadores y spawn points.
        Instantiate(Resources.Load<GameObject>("Prefabs/Player 1"), spawns[0].transform.position, Quaternion.identity);
        Instantiate(Resources.Load<GameObject>("Prefabs/Player 2"), spawns[1].transform.position, Quaternion.identity);
        Instantiate(Resources.Load<GameObject>("Prefabs/Player 3"), spawns[2].transform.position, Quaternion.identity);
        Instantiate(Resources.Load<GameObject>("Prefabs/Player 4"), spawns[3].transform.position, Quaternion.identity);

        weapons = new List<Weapon>();
        var filterList = Resources.LoadAll("Prefabs/Weapons").Select(x => (GameObject)x);
        weapons = filterList.Select(x => x.GetComponent<Weapon>()).ToList();
    }

    public int Register(Player player)
    {
        if (players == null) players = new List<Player>();
        players.Add(player);
        return players.Count - 1;
    }

    public void Unregister(Player player)
    {
        players.Remove(player);
    }
}
