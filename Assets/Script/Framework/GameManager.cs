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

    public event Action OnResetGame = delegate { };
    public event Action OnResetRound = delegate { };

    public int ScoreToReach;

    public Camera cam;

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

    RegisteredPlayers playerInfo;
    Transform[] spawns;

    void Start()
    {
        spawns = GameObject.Find("Stage").transform.Find("SpawnPoints").GetComponentsInChildren<Transform>().Where(x => x.name != "SpawnPoints").ToArray();

        playerInfo = Serializacion.LoadDataFromDisk<RegisteredPlayers>("Assets/Resources/Save Files/Registered Players.dat");

        for (int i = 0; i < playerInfo.playerControllers.Length; i++)
        {
            string path = "Assets/Resources/Save Files/Player " + (playerInfo.playerControllers[i] + 1) + ".dat";
            var URLs = Serializacion.LoadDataFromDisk<CharacterURLs>(path);

            var player = Instantiate(Resources.Load<GameObject>("Prefabs/Bodies/" + URLs.bodyURL), spawns[i].transform.position, Quaternion.identity).GetComponent<Player>();
            Instantiate(Resources.Load<GameObject>("Prefabs/Weapons/" + URLs.weaponURL), player.transform.position, Quaternion.identity, player.transform);
            var comp1 = Instantiate(Resources.Load<GameObject>("Prefabs/Skills/Complementary/" + URLs.complementaryURL[0]), player.transform.position, Quaternion.identity, player.transform);
            var comp2 = Instantiate(Resources.Load<GameObject>("Prefabs/Skills/Complementary/" + URLs.complementaryURL[1]), player.transform.position, Quaternion.identity, player.transform);
            Instantiate(Resources.Load<GameObject>("Prefabs/Skills/Defensive/" + URLs.defensiveURL), player.transform.position, Quaternion.identity, player.transform);

            comp1.GetComponent<ComplementarySkillBase>().RegisterInput(0);
            comp2.GetComponent<ComplementarySkillBase>().RegisterInput(1);

            player.gameObject.layer = LayerMask.NameToLayer("Player" + (playerInfo.playerControllers[i] + 1));
            player.gameObject.tag = "Player " + (playerInfo.playerControllers[i] + 1);
            foreach (Transform t in player.transform)
            {
                t.gameObject.layer = LayerMask.NameToLayer("Player" + (playerInfo.playerControllers[i] + 1));
                t.gameObject.tag = "Player " + (playerInfo.playerControllers[i] + 1);
            }
        }

        AddEvents();
    }


    void AddEvents()
    {
        EventManager.AddEventListener(PlayerEvents.Death, OnPlayerDeath);
        OnResetGame += DestroyStatic;
        OnResetRound += SpawnPlayers;
        OnResetRound += () => actualRound++;
        OnResetRound += EndRoundHandler.ResetTime;
    }

    void RemoveEvents()
    {
        EventManager.RemoveEventListener(PlayerEvents.Death, OnPlayerDeath);
    }

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

    public int Register(Player player)
    {
        if (players == null) players = new List<Player>();
        players.Add(player);

        var id = playerInfo.playerControllers[players.Count - 1];
        return id;
    }

    public void Unregister(Player player)
    {
        player.gameObject.SetActive(false);
        CheckIfSurvivor();
    }

    public void CheckIfSurvivor()
    {
        if (players.Where(x => x.isActiveAndEnabled).Count() == 1)
        {
            Players.First().UpdateScore(scoreBySurvivor);
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

            Invoke("ResetRound", 1);
        }
    }

    public void ResetRound()
    {
        OnResetRound();
    }

    public void SpawnPlayers()
    {
        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].gameObject.SetActive(true);
            Players[i].transform.position = spawns[i].transform.position;
            Players[i].ResetHP();
        }
    }

    public bool CheckIfReachedPoints()
    {
        return Players.Select(x => x.Score).OrderByDescending(x =>x).First() >= ScoreToReach;
    }

    public void EndGame()
    {
        EndgameManager.Instance.InitEndgame(1);
        EndRoundHandler.ResetTime();
    }

    public void ActivateCamera(bool activate)
    {
        cam.gameObject.SetActive(activate);
    }

    public void ResetGame()
    {
        OnResetGame();
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    void DestroyStatic()
    {
        StopAllCoroutines();
        instance = null;
        //Destroy(gameObject);
    }

    void Clean()
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
///
///20:34 ¯\_ツ_/¯
/// 
/// 20:45 T E N E M O S R E I N I C I O D E R O N D A S B O I I I I I I