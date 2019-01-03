using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SK_RepulsiveBattery : DefensiveSkillBase
{
    public AudioClip skillUse;

    public DMM_RepulsiveBatteryShield shield;

    public float maxCooldown;
    public float castTime;
    float _shieldDuration = 1f;

    bool _canTap = true;
    float _currentCooldown = 0;

    protected override void Start()
    {
        base.Start();
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
        if (activationAnim != null) activationAnim.Play();
        _owner.ApplyCastState(castTime);

        StartCoroutine(ShieldActivation());

        StartCoroutine(WaitForCastEnd(_owner.FinishedCasting));
    }

    public IEnumerator WaitForCastEnd(Func<bool> callback)
    {
        yield return new WaitUntil(callback);
        RepulsiveBatterySpawner.Instance.ObjectPool.GetObjectFromPool().Spawn(transform.position, _owner);
        _stateSource.PlayOneShot(skillUse);
        _currentCooldown = maxCooldown;
        _canTap = false;
    }

    public IEnumerator ShieldActivation()
    {
        shield.gameObject.SetActive(true);

        yield return new WaitForSeconds(_shieldDuration);

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
}
