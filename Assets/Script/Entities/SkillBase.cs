using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public abstract class SkillBase : MonoBehaviour
{
    protected SkillStateIndicator _feedback;
    protected Func<bool> _canUseSkill;
    protected PlayerUIModule _uiModule;

    protected AudioSource _stateSource;
    public AudioClip unavailableSound;

    protected Animation _activationAnim;
    protected ModuleParticleController _particleModule;

    protected virtual void Start()
    {
        _stateSource = GetComponent<AudioSource>();
        _activationAnim = GetComponent<Animation>();
        InitializeUseCondition();
        _uiModule = GetComponentInParent<PlayerUIModule>();
        _particleModule = GetComponent<ModuleParticleController>();
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

    public virtual string GetSkillName()
    {
        return this.ToString().Split('_').Skip(1).First().Split(')').First();
    }

    public abstract float GetCooldownPerc();

    public abstract void NotifyUIModule();

}

public enum SkillState { Unavailable, Available, Active, UserDisabled, ERROR }

