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

    protected virtual void Start()
    {
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
        var indic = GameObject.Instantiate(Resources.Load<SkillStateIndicator>("Prefabs/Skills/Helpers/ModuleFeedback"), transform);
        indic.InitializeIndicator(this);
        return (indic);

        /*var prefix = index == 0 ? "Left" : "Right";

        return _owner.GetComponentsInChildren<SkillStateIndicator>().Where(x => x.transform.parent.name == prefix + "ComplementaryModule").First();*/
    }
}
