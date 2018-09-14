﻿using System.Collections;
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
    GameObject _holder;
    GameObject[] _players;

    public string replaceStringName;
    public string replaceStringScore;

    public Transform[] spawnPos;

    GameObject _winner;
    bool _ending;

    void Start()
    {
        InitEndgame(.1f);
    }

    public void InitEndgame(float delay)
    {
        if (!_ending)
        {
            fader.gameObject.SetActive(true);
            StartCoroutine(FadeToBlack(delay));
            _ending = true;
        }

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

        yield return new WaitForSeconds(delay / 3);

        StartCoroutine(ReverseFade(delay));
    }

    void MovePlayersToPedestals()
    {
        _holder = GameObject.Find("PlayerContainer");
        _players = _holder.GetComponentsInChildren<Transform>(true).Select(x => x.gameObject).Where(x => x.name != _holder.name).ToArray();
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
        if (Input.GetKeyUp(KeyCode.R) && _ending) ResetGame();
        if (Input.GetKeyUp(KeyCode.Q) && _ending) QuitGame();
        if (Input.GetKeyUp(KeyCode.B) && _ending) GoBackToMenu();
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

    void Destruction()
    {
        Destroy(_holder);
        for (int i = 0; i < _players.Length; i++)
        {
            Destroy(_players[i].gameObject);
        }
    }

    void StartLoading(string sceneName)
    {
        StartCoroutine(LoadAsync());
    }

    IEnumerator LoadAsync()
    {
        var asyncOp = SceneManager.LoadSceneAsync("RingsStage", LoadSceneMode.Single);
        asyncOp.allowSceneActivation = false;

        while (asyncOp.progress <= .99f)
        {
            yield return new WaitForEndOfFrame();
        }

        Destruction();

        asyncOp.allowSceneActivation = true;
    }
}
