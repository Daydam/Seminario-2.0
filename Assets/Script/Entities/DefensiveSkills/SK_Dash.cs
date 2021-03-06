﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SK_Dash : DefensiveSkillBase
{
    public SO_Dash skillData;
    public AudioClip skillSound;

    SkillTrail _trail;

    bool _canTap = true;
    bool _isDashing;
    float _currentCooldown = 0;
    readonly float _debugMaxTimeOfCast = 2f;

    protected override void Start()
    {
        base.Start();

        skillData = Resources.Load<SO_Dash>("Scriptable Objects/Skills/Defensive/" + _owner.weightModule.prefix + GetSkillName() + _owner.weightModule.sufix) as SO_Dash;


        _trail = GetTrail();
    }

    SkillTrail GetTrail()
    {
        return GetComponentInChildren<SkillTrail>();
    }

    protected override void InitializeUseCondition()
    {
        _canUseSkill = () => !_owner.IsStunned && !_owner.IsDisarmed && !_isDashing && !_owner.IsCasting && !_owner.lockedByGame && _currentCooldown <= 0;
    }

    protected override void CheckInput()
    {
        if (_currentCooldown > 0) _currentCooldown -= Time.deltaTime;

        if (control.DefensiveSkill())
        {
            if (_canUseSkill())
            {
                if (_canTap)
                {
                    _trail.ShowTrails();

                    var dirV = _owner.transform.forward;

                    if (_owner.movDir != Vector3.zero)
                    {
                        dirV = _owner.movDir;
                    }

                    _canTap = false;
                    StartCoroutine(DashHandler(dirV.normalized));
                    _stateSource.PlayOneShot(skillSound, 2.5f);
                    _currentCooldown = skillData.maxCooldown;
                    NotifyUIModule();
                }
            }
            else
            {
                if (_canTap)
                {
                    _canTap = false;
                }
            }
        }
        else _canTap = true;
    }

    IEnumerator DashHandler(Vector3 dir)
    {
        _owner.CancelForces();
        _isDashing = true;
        var debugTime = 0f;
        var distanceTraveled = 0f;

        var amountByDelta = Time.fixedDeltaTime * skillData.dashDistance / skillData.dashTime;

        while (distanceTraveled < skillData.dashDistance)
        {
            distanceTraveled += amountByDelta;
            var nextPos = _owner.GetRigidbody.position + amountByDelta * dir;
            _owner.GetRigidbody.MovePosition(nextPos);

            yield return new WaitForFixedUpdate();

            debugTime += Time.fixedDeltaTime;
            if (debugTime >= _debugMaxTimeOfCast)
            {
                _trail.StopShowing();
                _isDashing = false;
                _canTap = false;
                yield break;
            }
        }

        _trail.StopShowing();

        _isDashing = false;
        _canTap = false;
    }

    public override void ResetRound()
    {
        _currentCooldown = 0;
        _isDashing = false;
        _trail.StopShowing();
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
