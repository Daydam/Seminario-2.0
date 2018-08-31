using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class EndgameManager : MonoBehaviour
{
    public Camera cam;
    public Image fader;
    public Text winnerText;
    public Text backToMenu;
    public Text restart;
    public Text quit;
    public GameObject playerCanvas;

    public string replaceStringName;
    public string replaceStringScore;

    public Transform[] spawnPos;

    Player _winner;
    bool _ending;

    static EndgameManager instance;
    public static EndgameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<EndgameManager>();
                if (instance == null)
                {
                    instance = new GameObject("new EndgameManager Object").AddComponent<EndgameManager>().GetComponent<EndgameManager>();
                }
            }
            return instance;
        }
    }

    public void InitEndgame(float delay)
    {
        GameManager.Instance.OnResetGame += DestroyStatic;
        fader.gameObject.SetActive(true);
        StartCoroutine(FadeToBlack(delay));
        _ending = true;
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

        MovePlayersToPedestals();
        ApplyTexts();
        ActivateCamera(true);
        GameManager.Instance.ActivateCamera(false);

        yield return new WaitForSeconds(delay/3);

        StartCoroutine(ReverseFade(delay));
    }

    void MovePlayersToPedestals()
    {
        var players = GameManager.Instance.Players.OrderByDescending(x => x.Score).ToArray();
        _winner = players.First();

        for (int i = 0; i < players.Length; i++)
        {
            players[i].gameObject.SetActive(true);
            players[i].lockedByGame = true;
            players[i].GetComponent<Rigidbody>().velocity = Vector3.zero;
            players[i].transform.position = spawnPos[i].position;
            players[i].transform.forward = -spawnPos[i].forward;
            players[i].ActivatePlayerEndgame(true, replaceStringName, replaceStringScore);
            players[i].StopVibrating();
            players[i].StopAllCoroutines();
            players[i].DeactivateCamera();
        }

        playerCanvas.SetActive(false);
          
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
        if (Input.GetKeyUp(KeyCode.R) && _ending) ResetGame();
    }

    void QuitGame()
    {

    }

    void ResetGame()
    {
        GameManager.Instance.ResetGame();

    }

    void GoBackToMenu()
    {

    }

    void DestroyStatic()
    {
        StopAllCoroutines();
        instance = null;
        //Destroy(gameObject);
    }

}
