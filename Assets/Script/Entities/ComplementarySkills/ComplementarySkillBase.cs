using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public abstract class ComplementarySkillBase : SkillBase
{
    protected Controller control;
    protected Player _owner;
    protected Func<bool> inputMethod;

    protected override void Start()
    {
        InitializeUseCondition();
        GameManager.Instance.OnResetRound += ResetRound;
    }

    protected virtual void Update()
    {
        CheckInput();
    }

    protected abstract void CheckInput();

    public void RegisterInput(int skillIndex)
    {
        _owner = GetComponentInParent<Player>();
        control = _owner.Control;
        if (skillIndex == 0) inputMethod = control.ComplimentarySkill1;
        if (skillIndex == 1) inputMethod = control.ComplimentarySkill2;

        _feedback = GetModuleFeedback(skillIndex);
    }

    public abstract void ResetRound();

    protected SkillStateIndicator GetModuleFeedback(int index)
    {
        var indic = Instantiate(Resources.Load<SkillStateIndicator>("Prefabs/Skills/Helpers/ModuleFeedback"), transform);
        indic.InitializeIndicator(this);
        return (indic);
    }
}
