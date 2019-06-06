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
    protected int _skillIndex;

    protected override void Start()
    {
        base.Start();
        GameManager.Instance.OnResetRound += ResetRound;
    }

    protected virtual void Update()
    {
        CheckInput();
    }

    protected abstract void CheckInput();

    public void RegisterInput(int skillIndex)
    {
        _skillIndex = skillIndex;
        _owner = GetComponentInParent<Player>();
        if(!_uiModule) _uiModule = GetComponentInParent<PlayerUIModule>();
        control = _owner.Control;
        if (skillIndex == 0) inputMethod = control.ComplimentarySkill1;
        if (skillIndex == 1) inputMethod = control.ComplimentarySkill2;

        _feedback = GetModuleFeedback(skillIndex);
        var indx = _skillIndex == 0 ? PlayerUIModule.SkillType.Complementary1 : PlayerUIModule.SkillType.Complementary2;
        _uiModule.InitializeComplementarySkill(this, indx);
    }

    public abstract void ResetRound();

    protected SkillStateIndicator GetModuleFeedback(int index)
    {
        var indic = Instantiate(Resources.Load<SkillStateIndicator>("Prefabs/Skills/Helpers/ModuleFeedback"), transform);
        indic.InitializeIndicator(this, GetComponentsInChildren<Renderer>(), false);
        return (indic);
    }

    public override void NotifyUIModule()
    {
        var indx = _skillIndex == 0 ? PlayerUIModule.SkillType.Complementary1 : PlayerUIModule.SkillType.Complementary2;
        _uiModule.UpdateSkillState(indx);
    }
}
