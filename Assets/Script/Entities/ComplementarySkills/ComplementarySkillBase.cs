using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ComplementarySkillBase : MonoBehaviour
{
    protected Controller control;
    protected Player _owner;
    protected Func<bool> inputMethod;

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
    }
}
