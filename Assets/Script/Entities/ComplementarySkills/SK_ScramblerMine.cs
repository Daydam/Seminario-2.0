using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SK_ScramblerMine : ComplementarySkillBase
{
    public float maxCooldown, duration, radius;

    float _currentCooldown = 0;

    protected override void CheckInput()
    {
        if (_currentCooldown > 0) _currentCooldown -= Time.deltaTime;
        else if (control.ComplimentarySkill1() && !_owner.IsStunned && !_owner.IsDisarmed)
        {
            ScramblerMineSpawner.Instance.ObjectPool.GetObjectFromPool().Spawn(_owner.transform.position, _owner.gameObject.transform.forward, duration, radius, _owner.gameObject.tag);
            _currentCooldown = maxCooldown;
        }
    }

    public override void ResetRound()
    {
        _currentCooldown = 0;
    }
}