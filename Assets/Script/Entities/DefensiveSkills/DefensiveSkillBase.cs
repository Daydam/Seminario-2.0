using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class DefensiveSkillBase : SkillBase
{
    protected Controller control;
    protected Player _owner;

    protected virtual void Start()
    {
        _owner = GetComponentInParent<Player>();
        control = _owner.Control;
        GameManager.Instance.OnResetRound += ResetRound;
        _feedback = GetModuleFeedback();
        _feedback.InitializeIndicator(this);
    }

    protected virtual void Update()
    {
        CheckInput();
    }

    protected abstract void CheckInput();
    public abstract void ResetRound();

    protected SkillStateIndicator GetModuleFeedback()
    {
        return _owner.GetComponentsInChildren<SkillStateIndicator>().Where(x => x.transform.parent.name == "DefensiveModule").First();
    }
}
