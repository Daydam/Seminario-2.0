using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;

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
    public Image fader;
    public Animator countdown;
    public readonly float fadeInDuration = 6f;
    public readonly float fadeOutDuration = 1f;

    public void Initialize(GameObject playerUIContainer, Action callback)
    {
        fader = playerUIContainer.transform.Find(blackFadeName).GetComponent<Image>();
        countdown = playerUIContainer.transform.Find(coundownName).GetComponent<Animator>();

        StartCoroutine(FadeIn(callback));
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

        callback();
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
