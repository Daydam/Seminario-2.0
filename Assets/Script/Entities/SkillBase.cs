using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public abstract class SkillBase : MonoBehaviour
{
    protected SkillStateIndicator _feedback;
    protected Func<bool> _canUseSkill;

    protected abstract void Start();
    protected abstract void InitializeUseCondition();

    /// <summary>
    /// * Verde > usable
    /// * Amarillo > en uso
    /// * Rojo > no se puede usar
    /// * Apagado > Dron disableado
    /// </summary>
    /// <returns> </returns>
    public abstract SkillState GetActualState();

}

public enum SkillState { Unavailable, Available, Active, Reloading, UserDisabled, ERROR }

