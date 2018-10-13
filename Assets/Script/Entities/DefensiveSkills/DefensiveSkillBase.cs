﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class DefensiveSkillBase : SkillBase
{
    protected Controller control;
    protected Player _owner;

    protected override void Start()
    {
        InitializeUseCondition();
        _owner = GetComponentInParent<Player>();
        control = _owner.Control;
        GameManager.Instance.OnResetRound += ResetRound;
        _feedback = GetModuleFeedback();
    }

    protected virtual void Update()
    {
        CheckInput();
    }

    protected abstract void CheckInput();
    public abstract void ResetRound();

    protected SkillStateIndicator GetModuleFeedback()
    {
        var indic = Instantiate(Resources.Load<SkillStateIndicator>("Prefabs/Skills/Helpers/ModuleFeedback"), transform);
        indic.InitializeIndicator(this, _owner.GetComponentInChildren<Renderer>());
        return (indic);
    }
}
