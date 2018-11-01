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
    public event Action StartRound = delegate { };

    public Camera cam;
    public GameObject[] playerCameras;
    public Texture[] cameraTexturesForTwoPlayers;

    public int actualRound = 1;
    public Dictionary<int, Tuple<Player, int>> roundResults;

    static GameManager instance;
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
            player.lockedByGame = true;
        }

        UIManager.Instance.Initialize(Players, StartFirstRound);
    }

    void Update()
    {

    }

    void AddEvents()
    {
        EventManager.Instance.AddEventListener(PlayerEvents.Death, OnPlayerDeath);
        OnChangeScene += DestroyStatic;
        OnResetRound += SpawnPlayers;
        OnResetRound += () => actualRound++;
        OnResetRound += EndRoundHandler.ResetTime;
    }

    void StartFirstRound()
    {
        AddEvents();
        StartRound();
    }

    void RemoveEvents()
    {
        EventManager.Instance.RemoveEventListener(PlayerEvents.Death, OnPlayerDeath);
    }


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

        if (CheckIfReachedPoints() && CheckIfOnlyWinner())
        {
            EndGame();
        }
        else
        {
            if (roundResults == null) roundResults = new Dictionary<int, Tuple<Player, int>>();
            roundResults[actualRound] = Tuple.Create(players.First(), players.First().Stats.Score);
            UIManager.Instance.EndRound(ResetRound);
            //do show winner and ui stuff
            //Invoke("ResetRound", 1);
        }
    }

    public void ResetRound()
    {
        OnResetRound();
        EventManager.Instance.DispatchEvent(GameEvents.RoundReset);
        UIManager.Instance.StartRound(StartRound);
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

    bool CheckIfOnlyWinner()
    {
        var list = Players.Where(x => x.Stats.Score >= gameRules.pointsToWin[playerInfo.playerControllers.Length - 2]);

        if (list.Count() == 1) return true;

        var firstScore = list.First().Stats.Score;

        return list.Where(x => x.Stats.Score == firstScore).Count() == 1;
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

    public bool HasTakenTheLead(int score)
    {
        var list = Players.Select(x => x.Stats.Score).OrderByDescending(x => x);

        return list.First() == score;
    }

    public void ScoreUpdate()
    {
        var list = Players.OrderByDescending(x => x.Stats.Score);
        var maxScores = list.Where(x => x.Stats.Score == list.First().Stats.Score);
        var others = Players.Except(maxScores);

        if (!others.Any())
        {
            foreach (var player in Players)
            {
                player.ScoreController.SetLeadingPlayer(false);
            }
        }
        else
        {
            foreach (var player in maxScores)
            {
                player.ScoreController.SetLeadingPlayer(true);
            }

            foreach (var player in others)
            {
                player.ScoreController.SetLeadingPlayer(false);
            }
        }

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
        var dyingPlayer = (Player)parameterContainer[0];
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
