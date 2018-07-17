using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SK_EMPCaltrop : ComplementarySkillBase
{
    public float maxCooldown, duration, amount, radius;
    public int maxCharges;

    int _actualCharges;
    float _currentCooldown = 0;
    bool _canUse = true;

    protected override void Start()
    {
        _actualCharges = maxCharges;
    }

    protected override void Update()
    {
        CheckForCharges();
        base.Update();
    }

    void CheckForCharges()
    {
        if (_actualCharges < maxCharges)
        {
            if (_currentCooldown > 0) _currentCooldown -= Time.deltaTime;
            else
            {
                _actualCharges++;
                _currentCooldown = maxCooldown;
            }
        }

    }

    protected override void CheckInput()
    {
        if (inputMethod())
        {
            if (_actualCharges > 0 && !_owner.IsStunned && !_owner.IsDisarmed && _canUse)
            {
                _actualCharges--;
                _canUse = false;
                LaunchCaltrop(duration, amount);
            }
        }
        else _canUse = true;
    }

    void LaunchCaltrop(float duration, float amount)
    {
        EMPCaltropSpawner.Instance.ObjectPool.GetObjectFromPool().Spawn(_owner.transform.position, _owner.gameObject.transform.forward, duration, amount, radius, _owner.gameObject.tag);
    }

    public override void ResetRound()
    {
        _actualCharges = maxCharges;
        _currentCooldown = 0;
        _canUse = true;
    }

    public override SkillState GetActualState()
    {
        var unavailable = _actualCharges <= 0;
        var reloading = _actualCharges > 0 && _actualCharges < maxCharges;
        var userDisabled = _owner.IsStunned || _owner.IsDisarmed;

        if (userDisabled) return SkillState.UserDisabled;
        else if (unavailable) return SkillState.Unavailable;
        else if (reloading) return SkillState.Reloading;
        else return SkillState.Available;
    }
}
