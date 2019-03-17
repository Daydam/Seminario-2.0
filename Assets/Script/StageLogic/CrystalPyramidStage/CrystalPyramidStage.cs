using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CrystalPyramidStage : StageBase
{
    public LaserPyramid pyramid;
    public CrystalRomboid[] crystalRomboids;
    int _actualIndex = 0;

    void Start()
    {
        pyramid = GetComponentInChildren<LaserPyramid>();
        crystalRomboids = GetComponentsInChildren<CrystalRomboid>().Reverse().ToArray();
        StartPyramidShrinkage();
    }

    public void StartPyramidShrinkage()
    {
        StartCoroutine(TestMugriento());
    }

    IEnumerator TestMugriento()
    {
        while (_actualIndex < crystalRomboids.Length - 1)
        {
            yield return new WaitForSeconds(15);
            SetRomboidDanger();
            yield return new WaitForSeconds(10);
            RomboidFall();
        }
    }

    public void SetRomboidDanger()
    {
        if (_actualIndex >= crystalRomboids.Length) return;
        crystalRomboids[_actualIndex].SetDanger();
    }

    public void RomboidFall()
    {
        if (_actualIndex >= crystalRomboids.Length) return;
        pyramid.OnFall(_actualIndex);
        crystalRomboids[_actualIndex].RomboidFall();
        _actualIndex++;
    }


    public override void ResetRound()
    {
        pyramid.OnResetRound();
        foreach (var item in crystalRomboids)
        {
            item.OnResetRound();
        }

        _actualIndex = 0;

        Invoke("StartPyramidShrinkage", 3);
    }
}
