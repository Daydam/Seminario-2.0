using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;
using Firepower.Events;

public class UIManager : MonoBehaviour
{
    static UIManager instance;
    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
                if (instance == null)
                {
                    instance = new GameObject("new UIManager Object").AddComponent<UIManager>().GetComponent<UIManager>();
                }
            }
            return instance;
        }
    }

    public readonly string blackFadeName = "BlackFade";
    public readonly string coundownName = "Countdown";
    public readonly string canvasName = "---CAMERA CANVAS---";

    GameObject _canvas;
    public Image fader;
    public Animator countdown;
    public readonly float fadeInDuration = 4f;
    public readonly float fadeOutDuration = 1f;
    public Text targetPointsText;
    Action _startRoundCallback;

    public void Initialize(IEnumerable<Player> players, Action callback, int pointsToWin)
    {
        _startRoundCallback = callback;
        EventManager.Instance.AddEventListener(UIEvents.StartRound, OnFinishedCountdown);
        _canvas = GameObject.Find(canvasName);
        fader = _canvas.transform.Find(blackFadeName).GetComponent<Image>();
        countdown = _canvas.transform.Find(coundownName).GetComponent<Animator>();
        targetPointsText.text = "Reach " + pointsToWin + " points!";

        ApplyTextes(players.ToArray());

        StartCoroutine(FadeIn(callback));
    }

    void OnFinishedCountdown(object[] paramsContainer)
    {
        _startRoundCallback();
    }

    void ApplyTextes(Player[] players)
    {
        var allTextes = _canvas.transform.Find(players.Length + " Player").Find("Textes").GetComponentsInChildren<Text>();

        for (int i = 0; i < allTextes.Length; i++)
        {
            allTextes[i].text = "PLAYER " + (players[i].myID + 1);
        }

    }

    public void StartRound(Action callback)
    {
        StartCoroutine(FadeIn(callback));
        countdown.Play("Active");
    }

    public void EndRound(Action callback)
    {
        StartCoroutine(FadeOut(callback));
    }

    IEnumerator FadeIn(Action callback)
    {
        float tick = 0.01666666666f;
        var yieldInstruction = new WaitForSecondsRealtime(tick);
        float elapsed = 0f;
        var amountByTick = tick / fadeInDuration;

        while (elapsed <= fadeInDuration)
        {
            yield return yieldInstruction;

            elapsed += tick;

            var col = fader.color;
            col.a -= amountByTick;
            fader.color = col;
        }

        //callback();
    }

    IEnumerator FadeOut(Action callback)
    {
        float tick = 0.01666666666f;
        var yieldInstruction = new WaitForSecondsRealtime(tick);
        float elapsed = 0f;
        var amountByTick = tick / fadeOutDuration;

        while (elapsed <= fadeOutDuration)
        {
            yield return yieldInstruction;

            elapsed += tick;

            var col = fader.color;
            col.a += amountByTick;
            fader.color = col;
        }

        callback();
    }
}
