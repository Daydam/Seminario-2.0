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
    Controller[] _controls;
    public GameObject playerText;
    public Color readyColor;
    public Color backToMenuColor;
    public Color notReadyColor;

    public string replaceStringName;
    public string replaceStringScore;

    public Transform[] spawnPos;
    public Renderer[] pedestals;

    bool _inputsAllowed = false;

    GameObject _winner;

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
        var organizedPlayers = playerInfo.playerControllers.OrderByDescending(a => playerInfo.playerScores[System.Array.IndexOf(playerInfo.playerControllers, a)]).ToArray();
        _players = new GameObject[organizedPlayers.Length];

        for (int i = 0; i < organizedPlayers.Length; i++)
        {
            var URLs = Serializacion.LoadJsonFromDisk<CharacterURLs>("Player " + (organizedPlayers[i] + 1));
            _controls = new Controller[4];
            _controls[i] = new Controller(i);

            //Dejo los objetos ccomo children del body por cuestiones de carga de los scripts. Assembler no debería generar problemas, ya que su parent objetivo sería el mismo.
            var player = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Bodies/" + URLs.bodyURL), spawnPos[i].position, Quaternion.identity);
            var weapon = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Weapons/" + URLs.weaponURL), player.transform.position, Quaternion.identity, player.transform);
            var comp1 = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Complementary 1/" + URLs.complementaryURL[0]), player.transform.position, Quaternion.identity, player.transform);
            var comp2 = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Complementary 2/" + URLs.complementaryURL[1]), player.transform.position, Quaternion.identity, player.transform);
            var def = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Defensive/" + URLs.defensiveURL), player.transform.position, Quaternion.identity, player.transform);
            var tx = Instantiate(playerText, player.transform.position, Quaternion.identity, player.transform);
            tx.transform.localPosition = Vector3.zero;
            tx.transform.Rotate(transform.up, 180);

            CharacterAssembler.Assemble(player.gameObject, def, comp1, comp2, weapon);

            player.gameObject.layer = LayerMask.NameToLayer("Player" + (playerInfo.playerControllers[i] + 1));
            player.gameObject.tag = "Player " + (playerInfo.playerControllers[i] + 1);
            foreach (Transform t in player.transform)
            {
                t.gameObject.layer = LayerMask.NameToLayer("Player" + (playerInfo.playerControllers[i] + 1));
                t.gameObject.tag = "Player " + (playerInfo.playerControllers[i] + 1);
            }

            player.transform.forward = spawnPos[i].forward;
            _players[i] = player;

            //tx.GetComponentInChildren<Text>().text = player.gameObject.tag + "\n" + organizedPlayers[i];
        }

        _winner = _players.First();

        var l = _players.Length;
        _resetInputs = new bool[l];
        playerIndexes = new PlayerIndex[l];
        previousGamePads = new GamePadState[l];
        currentGamePads = new GamePadState[l];
        for (int i = 0; i < currentGamePads.Length; i++)
        {
            currentGamePads[i] = GamePad.GetState((PlayerIndex)i);
        }

        for (int i = 0; i < _players.Length; i++)
        {
            var score = playerInfo.playerScores[System.Array.IndexOf(playerInfo.playerControllers, organizedPlayers[i])];
            EndgamePlayerText(_players[i], _players[i].gameObject.tag, score.ToString());
            _resetInputs[i] = false;
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
            currentGamePads[i] = GamePad.GetState((PlayerIndex)i);
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
        for (int i = 0; i < currentGamePads.Length; i++)
        {
            if (JoystickInput.allKeys[JoystickKey.START](previousGamePads[i], currentGamePads[i]))
            {
                _resetInputs[i] = !_resetInputs[i];
                var color = _resetInputs[i] ? readyColor : notReadyColor;
                pedestals[i].material.SetColor("_EmissionColor", color);
            }
        }

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
