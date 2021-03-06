﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SK_RepulsiveBattery : DefensiveSkillBase
{
    public SO_RepulsiveBattery skillData;

    public AudioClip skillUse;

    public DMM_RepulsiveBatteryShield shield;

    bool _canTap = true;
    float _currentCooldown = 0;

    protected override void Start()
    {
        base.Start();

        skillData = Resources.Load<SO_RepulsiveBattery>("Scriptable Objects/Skills/Defensive/" + _owner.weightModule.prefix + GetSkillName() + _owner.weightModule.sufix) as SO_RepulsiveBattery;

        shield = GetComponentInChildren<DMM_RepulsiveBatteryShield>(true);
        shield.gameObject.layer = _owner.gameObject.layer;
        shield.gameObject.SetActive(false);
    }

    protected override void InitializeUseCondition()
    {
        _canUseSkill = () => !_owner.IsStunned && !_owner.IsDisarmed && !_owner.IsCasting && !_owner.lockedByGame && _currentCooldown <= 0;
    }

    protected override void CheckInput()
    {
        if (_currentCooldown > 0) _currentCooldown -= Time.deltaTime;

        if (inputMethod())
        {
            if (_canUseSkill())
            {
                if (_canTap)
                {
                    _canTap = false;
                    UseSkill();
                }
            }
            //else _stateSource.PlayOneShot(unavailableSound);
        }
        else _canTap = true;
    }

    void UseSkill()
    {
        if (_activationAnim != null) _activationAnim.Play();
        _owner.ApplyCastState(skillData.castTime);

        _owner.ActivateRepulsion(skillData.shieldDuration, skillData.radius);

        StartCoroutine(ShieldActivation());

        StartCoroutine(WaitForCastEnd(_owner.FinishedCasting));
    }

    public IEnumerator WaitForCastEnd(Func<bool> callback)
    {
        yield return new WaitUntil(callback);
        RepulsiveBatterySpawner.Instance.ObjectPool.GetObjectFromPool().Spawn(transform.position, _owner, skillData);
        _stateSource.PlayOneShot(skillUse);
        _currentCooldown = skillData.maxCooldown;
        NotifyUIModule();
        _canTap = false;
    }

    public IEnumerator ShieldActivation()
    {
        _particleModule.OnShoot();
        shield.gameObject.SetActive(true);

        yield return new WaitForSeconds(skillData.shieldDuration);

        shield.gameObject.SetActive(false);
    }

    public override void ResetRound()
    {
        StopAllCoroutines();
        shield.gameObject.SetActive(false);
        _currentCooldown = 0;
    }

    public override SkillState GetActualState()
    {
        var unavailable = _currentCooldown > 0;
        var userDisabled = _owner.IsStunned || _owner.IsDisarmed;

        if (userDisabled) return SkillState.UserDisabled;
        else if (unavailable) return SkillState.Unavailable;
        else return SkillState.Available;
    }

    public override float GetCooldownPerc()
    {
        return Mathf.Lerp(1, 0, _currentCooldown / skillData.maxCooldown);
    }
}
