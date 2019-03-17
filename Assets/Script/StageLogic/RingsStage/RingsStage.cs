using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RingsStage : StageBase
{
    public EmpCloud empCloud;

    public MutatorRing[] levelRings;
    public int actualRing;
    public bool isLast;

    protected void Start()
    {
        empCloud = GameObject.FindObjectOfType<EmpCloud>();
    }

    public void DestroyRing()
    {
        levelRings[actualRing].DestroyRing();
        actualRing++;
        if (actualRing == levelRings.Length - 1) isLast = true;
    }

    public override void ResetRound()
    {
        empCloud.ResetRound();
        isLast = false;

        actualRing = 0;
        foreach (var item in levelRings)
        {
            item.ResetRound();
        }

    }
}
