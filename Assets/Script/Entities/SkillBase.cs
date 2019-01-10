using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public abstract class SkillBase : MonoBehaviour
{
    protected SkillStateIndicator _feedback;
    protected Func<bool> _canUseSkill;

    protected AudioSource _stateSource;
    public AudioClip unavailableSound;

    protected Animation activationAnim;

    protected virtual void Start()
    {
        _stateSource = GetComponent<AudioSource>();
        activationAnim = GetComponent<Animation>();
        InitializeUseCondition();
    }
    protected abstract void InitializeUseCondition();

    /// <summary>
    /// * Verde > usable
    /// * Amarillo > en uso
    /// * Rojo > no se puede usar
    /// * Apagado > Dron disableado
    /// </summary>
    /// <returns> </returns>
    public abstract SkillState GetActualState();

    protected virtual string GetSkillName()
    {
        return this.ToString().Split('_').Skip(1).First().Split(')').First();
    }

}

public enum SkillState { Unavailable, Available, Active, UserDisabled, ERROR }

