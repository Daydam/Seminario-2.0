using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DefensiveSkillBase : MonoBehaviour
{
    protected Controller control;
    protected Player _me;

    protected virtual void Start()
    {
        _me = GetComponentInParent<Player>();
        control = _me.Control;
    }

    protected virtual void Update()
    {
        CheckInput();
    }

    protected abstract void CheckInput();

}
