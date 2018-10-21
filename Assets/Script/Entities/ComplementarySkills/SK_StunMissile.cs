using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SK_StunMissile : ComplementarySkillBase
{
    public float maxCooldown;
    public float minRange, maxRange;

    bool _canTap = true;
    float _currentCooldown = 0;

    protected override void InitializeUseCondition()
    {
        _canUseSkill = () => !_owner.IsStunned && !_owner.IsDisarmed && !_owner.IsCasting && _currentCooldown <= 0;
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
                    StunMissileSpawner.Instance.ObjectPool.GetObjectFromPool().Spawn(transform.position, _owner.gameObject.transform.forward, maxRange, _owner.gameObject.tag);
                    _currentCooldown = maxCooldown;
                }
            }
            //else _stateSource.PlayOneShot(unavailableSound);
        }
        else _canTap = true;
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
