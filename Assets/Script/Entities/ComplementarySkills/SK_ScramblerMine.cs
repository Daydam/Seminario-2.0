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
        else if (control.ComplimentarySkill1() && !_me.IsStunned && !_me.IsDisarmed)
        {
            ScramblerMineSpawner.Instance.ObjectPool.GetObjectFromPool().Spawn(_me.transform.position, _me.gameObject.transform.forward, duration, radius, _me.gameObject.tag);
            _currentCooldown = maxCooldown;
        }
    }
}