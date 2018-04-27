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
        base.Start();
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
        if (control.ComplimentarySkill1())
        {
            if (_actualCharges > 0 && !_me.IsStunned && !_me.IsDisarmed && _canUse)
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
        EMPCaltropSpawner.Instance.ObjectPool.GetObjectFromPool().Spawn(_me.transform.position, _me.gameObject.transform.forward, duration, amount, radius, _me.gameObject.tag);
    }
}
