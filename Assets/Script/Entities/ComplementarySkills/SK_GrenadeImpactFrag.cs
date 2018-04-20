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
        else if (control.ComplimentarySkill1() && !_me.IsStunned && !_me.IsDisarmed)
        {
            //TODO: Que sea cargable el rango
            GrenadeImpactFragSpawner.Instance.ObjectPool.GetObjectFromPool().SpawnGrenade(transform.position, _me.gameObject.transform.forward, maxRange, _me.gameObject.tag);
            _currentCooldown = maxCooldown;
        }
    }
}
