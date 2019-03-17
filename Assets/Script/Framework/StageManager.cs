using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public sealed class StageManager : MonoBehaviour
{
    public static StageManager instance;

    public StageBase stage;

    public Action OnResetRound = delegate { }; 
    public Action OnDestroyStatic = delegate { }; 
    
    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        stage = GetComponent<StageBase>();
        InitializeDelegates();
        AddEvents();
    }

    void InitializeDelegates()
    {
        OnDestroyStatic += DestroyStatic;
        ResetRound();
    }

    public void ResetRound()
    {
        OnResetRound += stage.ResetRound;
    }

    void DestroyStatic()
    {
        stage.DestroyStatic();
        StopAllCoroutines();
        instance = null;
    }

    public void AddEvents()
    {
        GameManager.Instance.OnResetRound += OnResetRound;
        GameManager.Instance.OnChangeScene += OnDestroyStatic;
    }  
}
