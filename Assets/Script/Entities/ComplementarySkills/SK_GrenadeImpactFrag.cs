using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SK_GrenadeImpactFrag : ComplementarySkillBase
{
    public float maxCooldown;
    public float minRange, maxRange;

    float _currentCooldown = 0;

    protected override void CheckInput()
    {
        if (_currentCooldown > 0) _currentCooldown -= Time.deltaTime;
        else if (inputMethod() && !_owner.IsStunned && !_owner.IsDisarmed)
        {
            //TODO: Que sea cargable el rango
            GrenadeImpactFragSpawner.Instance.ObjectPool.GetObjectFromPool().SpawnGrenade(transform.position, _owner.gameObject.transform.forward, maxRange, _owner.gameObject.tag, _owner);
            _currentCooldown = maxCooldown;
        }
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
