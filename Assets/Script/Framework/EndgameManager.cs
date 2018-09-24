using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndgameManager : MonoBehaviour
{
    public Camera cam;
    public Image fader;
    public Text winnerText;
    public Text backToMenu;
    public Text restart;
    public Text quit;
    GameObject[] _players;

    public string replaceStringName;
    public string replaceStringScore;

    public Transform[] spawnPos;

    GameObject _winner;

    void Start()
    {
        InitEndgame(.1f);
    }

    public void InitEndgame(float delay)
    {
        LoadPlayers();
        ApplyTexts();
        ActivateCamera(true);

        fader.gameObject.SetActive(true);
        StartCoroutine(ReverseFade(delay));
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


        for (int i = 0; i < organizedPlayers.Length; i++)
        {
            var URLs = Serializacion.LoadJsonFromDisk<CharacterURLs>("Player " + (organizedPlayers[i] + 1));

            //Dejo los objetos ccomo children del body por cuestiones de carga de los scripts. Assembler no debería generar problemas, ya que su parent objetivo sería el mismo.
            var player = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Bodies/" + URLs.bodyURL), spawnPos[i].position, Quaternion.identity).GetComponent<Player>();
            var weapon = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Weapons/" + URLs.weaponURL), player.transform.position, Quaternion.identity, player.transform);
            var comp1 = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Complementary 1/" + URLs.complementaryURL[0]), player.transform.position, Quaternion.identity, player.transform);
            var comp2 = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Complementary 2/" + URLs.complementaryURL[1]), player.transform.position, Quaternion.identity, player.transform);
            var def = Instantiate(Resources.Load<GameObject>("Prefabs/CharacterSelection/Skills/Defensive/" + URLs.defensiveURL), player.transform.position, Quaternion.identity, player.transform);
            
            CharacterAssembler.Assemble(player.gameObject, def, comp1, comp2, weapon);

            player.gameObject.layer = LayerMask.NameToLayer("Player" + (playerInfo.playerControllers[i] + 1));
            player.gameObject.tag = "Player " + (playerInfo.playerControllers[i] + 1);
            foreach (Transform t in player.transform)
            {
                t.gameObject.layer = LayerMask.NameToLayer("Player" + (playerInfo.playerControllers[i] + 1));
                t.gameObject.tag = "Player " + (playerInfo.playerControllers[i] + 1);
            }


            player.transform.forward = -spawnPos[i].forward;
        }

        _winner = _players.First();

        for (int i = 0; i < _players.Length; i++)
        {
            var score = _players[i].transform.GetComponentInChildren<ScoreObject>();
            EndgamePlayerText(_players[i], score.gameObject.name);

            _players[i].transform.parent = null;
            _players[i].transform.position = spawnPos[i].position;
            _players[i].transform.forward = -spawnPos[i].forward;
        }
    }

    void EndgamePlayerText(GameObject playerText, string score)
    {
        var tx = playerText.GetComponentInChildren<Text>();
        tx.text = playerText.name + "\n" + score;
    }

    void ApplyTexts()
    {
        winnerText.gameObject.SetActive(true);
        backToMenu.gameObject.SetActive(true);
        restart.gameObject.SetActive(true);
        quit.gameObject.SetActive(true);

        winnerText.text = winnerText.text.Replace(replaceStringName, _winner.name);
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
        if (Input.GetKeyUp(KeyCode.R)) ResetGame();
        if (Input.GetKeyUp(KeyCode.Q)) QuitGame();
        if (Input.GetKeyUp(KeyCode.B)) GoBackToMenu();
    }

    void QuitGame()
    {
        Application.Quit();
    }

    void ResetGame()
    {
        StartLoading("RingsStage");
    }

    void GoBackToMenu()
    {
        StartLoading("CharacterSelection");
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
