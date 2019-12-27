using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CrystalPyramidStage : StageBase
{
    public LaserPyramid pyramid;
    public CrystalRomboid[] crystalRomboids;
    CrystalPyramidFallHandler _fallHandler;

    int _actualIndex;
    public float dangerTime = 15f;
    public float fallTime = 10f;

    void Start()
    {
        pyramid = GetComponentInChildren<LaserPyramid>();
        crystalRomboids = GetComponentsInChildren<CrystalRomboid>().ToArray();

        _fallHandler = GetComponent<CrystalPyramidFallHandler>();

        StartPyramidShrinkage();
    }

    public void StartPyramidShrinkage()
    {
        StartCoroutine(StageMutationCoroutine());
    }

    IEnumerator StageMutationCoroutine()
    {
        while (_actualIndex < crystalRomboids.Length - 1)
        {
            yield return new WaitForSeconds(dangerTime);
            SetRomboidDanger();
            yield return new WaitForSeconds(fallTime);
            RomboidFall();
        }
    }

    public void SetRomboidDanger()
    {
        if (_actualIndex >= crystalRomboids.Length) return;
        crystalRomboids[_actualIndex].SetDanger();
        _fallHandler.SetDangerState(_actualIndex);

    }

    public void RomboidFall()
    {
        if (_actualIndex >= crystalRomboids.Length) return;
        pyramid.OnFall(_actualIndex);
        crystalRomboids[_actualIndex].RomboidFall();
        _actualIndex++;
        _fallHandler.SetFallState(_actualIndex);
    }


    public override void ResetRound()
    {
        StopAllCoroutines();
        CancelInvoke();

        pyramid.OnResetRound();
        foreach (var item in crystalRomboids)
        {
            item.OnResetRound();
        }

        _actualIndex = 0;

        Invoke("StartPyramidShrinkage", 3);
        _fallHandler.ResetRound();
    }
}
