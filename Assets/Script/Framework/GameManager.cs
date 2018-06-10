using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Events;
using System;

public class GameManager : MonoBehaviour
{
    public int scoreByKill = 2;
    public int scoreByFalling = -1;
    public int scoreByThrownOff = -2;
    public int scoreBySurvivor = 1;

    public int ScoreToReach = 50;


    public int actualRound = 1;
    public Dictionary<int, Tuple<Player, int>> roundResults;

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

    Dictionary<int, Player> _allPlayers;

    RegisteredPlayers playerInfo;
    Transform[] spawns;

    void Start()
    {
        spawns = GameObject.Find("Stage").transform.Find("SpawnPoints").GetComponentsInChildren<Transform>().Where(x => x.name != "SpawnPoints").ToArray();

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

        AddEvents();
    }


    void AddEvents()
    {
        EventManager.AddEventListener(PlayerEvents.Death, OnPlayerDeath);
    }

    void RemoveEvents()
    {
        EventManager.AddEventListener(PlayerEvents.Death, OnPlayerDeath);
    }

    #region Cambios Iván 10/6

    #region EventCallbacks
    /// <summary>
    /// 0 - PlayerDying
    /// 1 - DeathType
    /// 2 - WasPushed?
    /// 3 - PlayerKiller (tag del jugador)
    /// </summary>
    void OnPlayerDeath(object[] parameterContainer)
    {
        var dyingPlayer = players.Where(x => x == (Player)parameterContainer[0]).First();
        var deathType = (DeathType)parameterContainer[1];
        var wasPushed = (bool)parameterContainer[2];
        var playerKiller = players.Where(x => x.tag == (string)parameterContainer[3]).First();

        if (deathType == DeathType.LaserGrid)
        {
            var score = wasPushed ? scoreByThrownOff : scoreByFalling;
            dyingPlayer.UpdateScore(score);
        }

        if (dyingPlayer != playerKiller)
        {
            playerKiller.UpdateScore(scoreByKill);
        }

        Unregister(dyingPlayer);
    }

    #endregion
    #endregion

    public int Register(Player player)
    {
        if (players == null) players = new List<Player>();
        players.Add(player);

        var id = playerInfo.playerControllers[players.Count - 1];
        AddToRegistry(player, id);
        return id;
    }

    public void Register(Player player, int id)
    {
        if (players == null) players = new List<Player>(4);
        players[id] = player;
    }

    void AddToRegistry(Player player, int id)
    {
        if (_allPlayers == null) _allPlayers = new Dictionary<int, Player>();

        if (_allPlayers.ContainsKey(id)) return;
        else _allPlayers.Add(id, player);
    }

    public void Unregister(Player player)
    {
        players.Remove(player);
        CheckIfSurvivor();

    }
    public void UnregisterAll()
    {
        players = null;
    }

    public void CheckIfSurvivor()
    {
        if (players.Count == 1)
        {
            //bla
            players.First().UpdateScore(scoreBySurvivor);
            EndRound();
        }
    }

    public void EndRound()
    {
        EndRoundHandler.ApplyTimeChanges();

        if (CheckIfReachedPoints())
        {
            //do ui endgame stuff
            EndGame();
        }
        else
        {
            if (roundResults == null) roundResults = new Dictionary<int, Tuple<Player, int>>();
            roundResults[actualRound] = Tuple.Create(players.First(), players.First().Score);

            //do show winner and ui stuff

            ResetRound();
        }
    }

    public void ResetRound()
    {
        SpawnPlayers();

        actualRound++;

        EndRoundHandler.ResetTime();
    }

    public void SpawnPlayers()
    {
        for (int i = 1; i <= _allPlayers.Count; i++)
        {
            Register(_allPlayers[i], i);
            Players[i].transform.position = spawns[i].transform.position;
            Players[i].gameObject.SetActive(true);
        }
    }

    public bool CheckIfReachedPoints()
    {
        return players.OrderBy(x => x.Score).Select(x => x.Score).First() >= ScoreToReach;
    }

    public void EndGame()
    {

    }
}

///PARA GASTON SI LO LLEGA A VER
///D-VA: ADD CALLBACK, EH!
///
///16:04 (ⴲ ͜ʖⴲ)
///
///16:04 ( ͡° ͜ʖ ͡°)
///
///16:50 ಠ_ಠ
///
///16:55 (╯ಠᴥಠ）╯︵ ┻━┻