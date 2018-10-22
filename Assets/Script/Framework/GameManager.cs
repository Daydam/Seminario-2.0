using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Events;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public SO_GameRules gameRules;

    bool _gameEnded = false;

    public event Action OnChangeScene = delegate { };
    public event Action OnResetRound = delegate { };

    public Camera cam;
    public GameObject[] playerCameras;
    public Texture[] cameraTexturesForTwoPlayers;

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

    public AudioSource AudioSource
    {
        get
        {
            if (_audioSource == null) _audioSource = GetComponent<AudioSource>();
            return _audioSource;
        }
    }

    RegisteredPlayers playerInfo;
    Transform[] spawns;
    AudioSource _audioSource;

    void Start()
    {
        spawns = GameObject.Find("Stage").transform.Find("SpawnPoints").GetComponentsInChildren<Transform>().Where(x => x.name != "SpawnPoints").ToArray();

        playerInfo = Serializacion.LoadJsonFromDisk<RegisteredPlayers>("Registered Players");
        playerInfo.playerStats = new PlayerStats[playerInfo.playerControllers.Length];
        playerCameras[playerInfo.playerControllers.Length - 2].SetActive(true);
        if (playerInfo.playerControllers.Length == 2)
        {
            for (int i = 0; i < 2; i++)
            {
                Camera c = GameObject.Find("Camera_P" + (i + 1)).GetComponent<Camera>();
                cameraTexturesForTwoPlayers[i].width = 1280;
                c.rect = new Rect(0, 0, 2, 1);
            }
        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                Camera c = GameObject.Find("Camera_P" + (i + 1)).GetComponent<Camera>();
                cameraTexturesForTwoPlayers[i].width = 640;
                c.rect = new Rect(0, 0, 1, 1);
            }
        }

        //Setting the mode!
        gameRules = Resources.Load("Scriptable Objects/GameMode_" + playerInfo.gameMode) as SO_GameRules;

        for (int i = 0; i < playerInfo.playerControllers.Length; i++)
        {
            var URLs = Serializacion.LoadJsonFromDisk<CharacterURLs>("Player " + (playerInfo.playerControllers[i] + 1));

            //Dejo los objetos ccomo children del body por cuestiones de carga de los scripts. Assembler no debería generar problemas, ya que su parent objetivo sería el mismo.
            var player = Instantiate(Resources.Load<GameObject>("Prefabs/Bodies/" + URLs.bodyURL), spawns[i].transform.position, Quaternion.identity).GetComponent<Player>();
            var weapon = Instantiate(Resources.Load<GameObject>("Prefabs/Weapons/" + URLs.weaponURL), player.transform.position, Quaternion.identity, player.transform);
            var comp1 = Instantiate(Resources.Load<GameObject>("Prefabs/Skills/Complementary 1/" + URLs.complementaryURL[0]), player.transform.position, Quaternion.identity, player.transform);
            var comp2 = Instantiate(Resources.Load<GameObject>("Prefabs/Skills/Complementary 2/" + URLs.complementaryURL[1]), player.transform.position, Quaternion.identity, player.transform);
            var def = Instantiate(Resources.Load<GameObject>("Prefabs/Skills/Defensive/" + URLs.defensiveURL), player.transform.position, Quaternion.identity, player.transform);

            CharacterAssembler.Assemble(player.gameObject, def, comp1, comp2, weapon);
            player.transform.forward = spawns[i].forward;

            comp1.GetComponent<ComplementarySkillBase>().RegisterInput(0);
            comp2.GetComponent<ComplementarySkillBase>().RegisterInput(1);

            player.gameObject.layer = LayerMask.NameToLayer("Player" + (playerInfo.playerControllers[i] + 1));
            player.gameObject.tag = "Player " + (playerInfo.playerControllers[i] + 1);
            foreach (Transform t in player.transform)
            {
                t.gameObject.layer = LayerMask.NameToLayer("Player" + (playerInfo.playerControllers[i] + 1));
                t.gameObject.tag = "Player " + (playerInfo.playerControllers[i] + 1);
            }

            player.Stats.Score = 0;

            CamFollow cam = GameObject.Find("Camera_P" + (i + 1)).GetComponent<CamFollow>();
            cam.AssignTarget(player);
        }

        AddEvents();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            Players[0].Stats.Score = 6;
            Players[1].Stats.Score = 5;
            Players[2].Stats.Score = 9;
            Players[3].Stats.Score = 13;
        }
        else if (Input.GetKeyUp(KeyCode.Y))
        {
            Players[1].Stats.Score = 6;
            Players[0].Stats.Score = 5;
            Players[2].Stats.Score = 9;
            Players[3].Stats.Score = 13;
        }
        else if (Input.GetKeyUp(KeyCode.I))
        {
            Players[2].Stats.Score = 6;
            Players[1].Stats.Score = 5;
            Players[0].Stats.Score = 9;
            Players[3].Stats.Score = 13;
        }
    }

    void AddEvents()
    {
        EventManager.Instance.AddEventListener(PlayerEvents.Death, OnPlayerDeath);
        OnChangeScene += DestroyStatic;
        OnResetRound += SpawnPlayers;
        OnResetRound += () => actualRound++;
        OnResetRound += EndRoundHandler.ResetTime;
    }

    void RemoveEvents()
    {
        EventManager.Instance.RemoveEventListener(PlayerEvents.Death, OnPlayerDeath);
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
        dyingPlayer.Stats.Deaths++;

        if (deathType == DeathType.LaserGrid)
        {
            var score = wasPushed ? gameRules.pointsPerDrop : gameRules.pointsPerSuicide;
            dyingPlayer.UpdateScore(score);
        }

        if (dyingPlayer != playerKiller)
        {
            playerKiller.UpdateScore(gameRules.pointsPerKill);
            playerKiller.Stats.Kills++;
        }

        Unregister(dyingPlayer);
    }

    #endregion

    public int Register(Player player)
    {
        if (Players == null) players = new List<Player>();
        Players.Add(player);

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
        var alivePlayers = Players.Where(x => x.gameObject.activeInHierarchy);
        if (alivePlayers.Count() == 1)
        {
            alivePlayers.First().UpdateScore(gameRules.pointsForLast);
            alivePlayers.First().Stats.Survived++;
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
            roundResults[actualRound] = Tuple.Create(players.First(), players.First().Stats.Score);

            //do show winner and ui stuff

            Invoke("ResetRound", 1);
        }
    }

    public void ResetRound()
    {
        OnResetRound();
        EventManager.Instance.DispatchEvent(GameEvents.RoundReset);
    }

    public void SpawnPlayers()
    {
        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].gameObject.SetActive(true);
            Players[i].transform.position = spawns[i].transform.position;
            Players[i].transform.forward = spawns[i].forward;
            Players[i].ResetHP();
            Players[i].StopAllCoroutines();
        }
    }

    public bool CheckIfReachedPoints()
    {
        return Players.Select(x => x.Stats.Score).OrderByDescending(x => x).First() >= gameRules.pointsToWin[playerInfo.playerControllers.Length - 2];
    }

    public void EndGame()
    {
        EndRoundHandler.ResetTime();
        _gameEnded = true;
        playerInfo.playerStats = new PlayerStats[players.Count];
        for (int i = 0; i < Players.Count; i++)
        {
            Players[i].StopVibrating();
            playerInfo.playerStats[i] = Players[i].Stats;
        }
        Serializacion.SaveJsonToDisk(playerInfo, "Registered Players");
        StartCoroutine(EndGameCoroutine());
    }

    IEnumerator EndGameCoroutine()
    {
        var asyncOp = SceneManager.LoadSceneAsync("EndgameScreen", LoadSceneMode.Single);
        asyncOp.allowSceneActivation = true;

        while (asyncOp.progress <= .99f)
        {
            yield return new WaitForEndOfFrame();
        }
    }

    public void ActivateCamera(bool activate)
    {
        cam.gameObject.SetActive(activate);
    }

    void DestroyStatic()
    {
        RemoveEvents();
        StopAllCoroutines();
        instance = null;
        players = null;
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
