using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SK_ScramblerMine : ComplementarySkillBase
{
    public float maxCooldown, duration, radius;

    float _currentCooldown = 0;
    DMM_ScramblerMine _mine;

    bool _inUse;

    //TODO HACER QUE NO SE PUEDA TIRAR MÁS DE UNA SCRAMBLER

    protected override void CheckInput()
    {
        if (_currentCooldown > 0) _currentCooldown -= Time.deltaTime;
        else if (inputMethod() && !_owner.IsStunned && !_owner.IsDisarmed)
        {
            if (MineActive()) _mine.Explode(true);
            
            _mine = ScramblerMineSpawner.Instance.ObjectPool.GetObjectFromPool().Spawn(_owner.transform.position, _owner.gameObject.transform.forward, duration, radius, _owner.gameObject.tag);
            _currentCooldown = maxCooldown;
        }
    }

    public override void ResetRound()
    {
        _currentCooldown = 0;
    }

    bool MineActive()
    {
        return _mine != null && _mine.gameObject.activeInHierarchy;
    }

    public override SkillState GetActualState()
    {
        var unavailable = _currentCooldown > 0;
        var userDisabled = _owner.IsStunned || _owner.IsDisarmed;
        var active = MineActive();

        if (userDisabled) return SkillState.UserDisabled;
        else if (unavailable) return SkillState.Unavailable;
        else if (active) return SkillState.Active;
        else return SkillState.Available;
    }
}