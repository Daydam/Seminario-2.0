using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SK_RepulsiveBattery : ComplementarySkillBase
{
    public float maxCooldown;
    public float castTime;

    float _currentCooldown = 0;

    protected override void CheckInput()
    {
        if (_currentCooldown > 0) _currentCooldown -= Time.deltaTime;
        else if (inputMethod() && !_owner.IsStunned && !_owner.IsDisarmed && !_owner.IsCasting)
        {
            UseSkill();
        }
    }

    void UseSkill()
    {
        _owner.ApplyCastState(castTime);
        StartCoroutine(WaitForCastEnd(_owner.FinishedCasting));
    }

    public IEnumerator WaitForCastEnd(Func<bool> callback)
    {
        yield return new WaitUntil(callback);
        RepulsiveBatterySpawner.Instance.ObjectPool.GetObjectFromPool().Spawn(transform.position, _owner);
        _currentCooldown = maxCooldown;
    }

    public override void ResetRound()
    {
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
