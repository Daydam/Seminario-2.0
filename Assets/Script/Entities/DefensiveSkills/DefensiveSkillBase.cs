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
    }

    protected virtual void Update()
    {
        CheckInput();
    }

    protected abstract void CheckInput();
    public abstract void ResetRound();

    protected SkillStateIndicator GetModuleFeedback()
    {
        var indic = GameObject.Instantiate(Resources.Load<SkillStateIndicator>("Prefabs/Skills/Helpers/ModuleFeedback"), transform);
        indic.InitializeIndicator(this, _owner.GetComponentInChildren<SkinnedMeshRenderer>());
        return (indic);

        //return _owner.GetComponentsInChildren<SkillStateIndicator>().Where(x => x.transform.parent.name == "DefensiveModule").First();
    }
}
