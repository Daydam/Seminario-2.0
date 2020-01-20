using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public abstract class DefensiveSkillBase : SkillBase
{
    protected Controller control;
    protected Player _owner;
    protected Func<bool> inputMethod;

    protected override void Start()
    {
        base.Start();
        _owner = GetComponentInParent<Player>();
        control = _owner.Control;
        inputMethod = control.DefensiveSkill;
        GameManager.Instance.OnResetRound += ResetRound;
        _feedback = GetModuleFeedback();
        _uiModule.InitializeDefensiveSkill(this);
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
        indic.InitializeIndicator(this, _owner.GetComponentsInChildren<Renderer>(), true);
        return (indic);
    }

    public override void NotifyUIModule()
    {
        _uiModule.UpdateSkillState(PlayerUIModule.SkillType.Defensive);
    }
}
