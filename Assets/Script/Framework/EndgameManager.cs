using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using XInputDotNetPure;
using System;

public class EndgameManager : MonoBehaviour
{
    public Camera cam;
    public Image fader;
    public Text winnerText;
    public Text backToMenu;
    public Text restart;
    public Text quit;
    GameObject[] _players;
    public GameObject playerText;
    public Color readyColor;
    public Color backToMenuColor;
    public Color notReadyColor;

    public string replaceStringName;
    public string replaceStringScore;

    public Transform[] spawnPos;
    public Renderer[] pedestals;
    public string pedestalName = "Pedestal";

    bool _inputsAllowed = false;

    GameObject _winner;

    //Lo nuevo de DJ Tincho. Deaths no la implemento por razones lógicas... todos mueren la misma cantidad de veces menos el ganador.
    int mostKills;
    int mostDrops;
    int mostSurvived;
    int mostDamageDealt;
    int leastDamageTaken;

    #region Pero mirá como está este copy & paste del CharacterSelectionManager papá
    bool[] _resetInputs;
    //XInput
    PlayerIndex[] playerIndexes;
    GamePadState[] previousGamePads;
    GamePadState[] currentGamePads;
    #endregion

    void Start()
    {
        LoadPlayers();
        ApplyTexts();
        ActivateCamera(true);

        fader.gameObject.SetActive(true);

        _inputsAllowed = true;
    }

    IEnumerator FadeToBlack(float delay)
    {
        var fadeInstruction = new WaitForEndOfFrame();

        var elapsedTime = 0f;
        Color c = fader.color;
        c.a = 0;
        fader.color = c;
        while (elapsedTime < delay)
        {
            yield return fadeInstruction;
            elapsedTime += Time.deltaTime;
            c.a = Mathf.Clamp01(elapsedTime / delay);
            fader.color = c;
        }

        yield return new WaitForSeconds(delay / 3);

    }

    void LoadPlayers()
    {
        var playerInfo = Serializacion.LoadJsonFromDisk<RegisteredPlayers>("Registered Players");
        var playerControllersOrdered = playerInfo.playerControllers.OrderByDescending(x => playerInfo.playerStats[Array.IndexOf(playerInfo.playerControllers, x)].Score).ToArray();
        var playerScoresOrdered = playerInfo.playerStats.OrderByDescending(y => y.Score).ToArray();
        _players = new GameObject[playerControllersOrdered.Length];

        for (int i = 0; i < playerControllersOrdered.Length; i++)
        {
            var URLs = Serializacion.LoadJsonFromDisk<CharacterURLs>("Player " + (playerControllersOrdered[i] + 1));

            //Dejo los objetos como children del body por cuestiones de carga de los scripts. Assembler no debería generar problemas, ya que su parent objetivo sería el mismo.
            var player = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Bodies/" + URLs.bodyURL), spawnPos[i].position, Quaternion.identity);
            var weapon = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Weapons/" + URLs.weaponURL), player.transform.position, Quaternion.identity, player.transform);
            var comp1 = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Complementary 1/" + URLs.complementaryURL[0]), player.transform.position, Quaternion.identity, player.transform);
            var comp2 = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Complementary 2/" + URLs.complementaryURL[1]), player.transform.position, Quaternion.identity, player.transform);
            var def = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Defensive/" + URLs.defensiveURL), player.transform.position, Quaternion.identity, player.transform);
            var tx = Instantiate(playerText, player.transform.position, Quaternion.identity, player.transform);
            tx.transform.localPosition = Vector3.zero;
            tx.transform.Rotate(transform.up, 180);

            CharacterAssembler.Assemble(player.gameObject, def, comp1, comp2, weapon);

            player.gameObject.layer = LayerMask.NameToLayer("Player" + (playerControllersOrdered[i] + 1));
            player.gameObject.tag = "Player " + (playerControllersOrdered[i] + 1);
            foreach (Transform t in player.transform)
            {
                t.gameObject.layer = LayerMask.NameToLayer("Player" + (playerControllersOrdered[i] + 1));
                t.gameObject.tag = "Player " + (playerControllersOrdered[i] + 1);
            }

            player.transform.forward = spawnPos[i].forward;
            _players[i] = player;
        }

        _winner = _players.First();

        var l = _players.Length;
        _resetInputs = new bool[l];
        playerIndexes = new PlayerIndex[l];
        previousGamePads = new GamePadState[l];
        currentGamePads = new GamePadState[l];
        for (int i = 0; i < currentGamePads.Length; i++)
        {
            playerIndexes[i] = (PlayerIndex)playerControllersOrdered[i];
            currentGamePads[i] = GamePad.GetState(playerIndexes[i]);
        }

        for (int i = 0; i < _players.Length; i++)
        {
            var score = playerScoresOrdered[i].Score;
            EndgamePlayerText(_players[i], _players[i].gameObject.tag, score.ToString());
            _resetInputs[i] = false;
        }
        
        mostKills = playerInfo.playerControllers.OrderByDescending(x => playerInfo.playerStats[Array.IndexOf(playerInfo.playerControllers, x)].Kills).First();
        var killAmount = playerInfo.playerStats.OrderByDescending(y => y.Kills).First().Kills;
        mostSurvived = playerInfo.playerControllers.OrderByDescending(x => playerInfo.playerStats[Array.IndexOf(playerInfo.playerControllers, x)].Survived).First();
        var survivedAmount = playerInfo.playerStats.OrderByDescending(y => y.Survived).First().Survived;
        mostDamageDealt = playerInfo.playerControllers.OrderByDescending(x => playerInfo.playerStats[Array.IndexOf(playerInfo.playerControllers, x)].DamageDealt).First();
        var damageDealtAmount = playerInfo.playerStats.OrderByDescending(y => y.DamageDealt).First().DamageDealt;
        leastDamageTaken = playerInfo.playerControllers.OrderBy(x => playerInfo.playerStats[Array.IndexOf(playerInfo.playerControllers, x)].DamageTaken).First();
        var damageTakenAmount = playerInfo.playerStats.OrderBy(y => y.DamageTaken).First().DamageTaken;

        print("Psycho Killer Award goes to: Player " + (mostKills + 1) + " with " + killAmount + " kills");
        print("Survived Award goes to: Player " + (mostSurvived + 1) + " with " + survivedAmount + " rounds survived");
        print("Dealer Award goes to: Player " + (mostDamageDealt + 1) + " with " + damageDealtAmount + " damage dealt");
        print("Bulletproof Award goes to: Player " + (leastDamageTaken + 1) + " with " + damageTakenAmount + " damage taken");

        HideUnusedPedestals();
    }

    void HideUnusedPedestals()
    {
        if (currentGamePads.Length < pedestals.Length)
        {
            var filteredPedestals = pedestals.Skip(currentGamePads.Length);
            foreach (var item in filteredPedestals)
            {
                item.gameObject.SetActive(false);
            }
        }
    }

    void EndgamePlayerText(GameObject playerText, string name, string score)
    {
        var tx = playerText.GetComponentInChildren<Text>();
        tx.text = name + "\n" + score;
    }

    void ApplyTexts()
    {
        winnerText.gameObject.SetActive(true);
        backToMenu.gameObject.SetActive(true);
        restart.gameObject.SetActive(true);
        quit.gameObject.SetActive(true);

        winnerText.text = winnerText.text.Replace(replaceStringName, _winner.tag);
    }

    public void ActivateCamera(bool activate)
    {
        cam.gameObject.SetActive(activate);
    }

    IEnumerator ReverseFade(float delay)
    {
        var fadeInstruction = new WaitForEndOfFrame();

        float elapsedTime = 0.0f;
        Color c = fader.color;
        while (elapsedTime < delay)
        {
            yield return fadeInstruction;
            elapsedTime += Time.deltaTime;
            c.a = 1.0f - Mathf.Clamp01(elapsedTime / delay);
            fader.color = c;
        }
    }

    void Update()
    {
        if (!_inputsAllowed) return;

        for (int i = 0; i < currentGamePads.Length; i++)
        {
            previousGamePads[i] = currentGamePads[i];
            currentGamePads[i] = GamePad.GetState(playerIndexes[i]);

            if (JoystickInput.allKeys[JoystickKey.START](previousGamePads[i], currentGamePads[i]))
            {
                _resetInputs[i] = !_resetInputs[i];
                var color = _resetInputs[i] ? readyColor : notReadyColor;
                pedestals[i].material.SetColor("_EmissionColor", color);
            }
        }

        if (ResetGame())
        {
            _inputsAllowed = false;
            StartLoading("RingsStage");
        }
        else if (BackToMenu())
        {
            _inputsAllowed = false;
            StartLoading("Character Selection");
        }
    }

    #region Enfermedad mágica mística me cago en dios
    bool ResetGame()
    {
        return _resetInputs.All(x => x);
    }

    bool BackToMenu()
    {
        for (int i = 0; i < currentGamePads.Length; i++)
        {
            if (JoystickInput.allKeys[JoystickKey.BACK](previousGamePads[i], currentGamePads[i]))
            {
                pedestals[i].material.SetColor("_EmissionColor", backToMenuColor);
                return true;
            }
        }
        return false;
    }
    #endregion

    void QuitGame()
    {
        Application.Quit();
    }

    void StartLoading(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    IEnumerator LoadSceneCoroutine(string sceneName)
    {
        var asyncOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        asyncOp.allowSceneActivation = true;

        while (asyncOp.progress <= .99f)
        {
            yield return new WaitForEndOfFrame();
        }
    }
}
