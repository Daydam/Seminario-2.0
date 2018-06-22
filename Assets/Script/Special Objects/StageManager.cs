using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;
    public EmpCloud empCloud;

    public MutatorRing[] levelRings;
    public int actualRing;
    public bool isLast;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        empCloud = GameObject.FindObjectOfType<EmpCloud>();
        GameManager.Instance.OnResetRound += ResetRound;
        GameManager.Instance.OnResetGame += DestroyStatic;
    }

    public void DestroyRing()
    {
        levelRings[actualRing].DestroyRing();
        actualRing++;
        if (actualRing == levelRings.Length - 1) isLast = true;
    }

    public void ResetRound()
    {
        empCloud.ResetRound();
        isLast = false;

        actualRing = 0;
        foreach (var item in levelRings)
        {
            item.ResetRound();
        }

    }

    void DestroyStatic()
    {
        StopAllCoroutines();
        instance = null;
        //Destroy(gameObject);
    }
}
