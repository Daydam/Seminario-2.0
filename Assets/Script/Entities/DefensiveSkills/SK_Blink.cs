using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SK_Blink : DefensiveSkillBase
{
    public float maxCooldown;
    public float blinkDistance;
    SkillTrail _trail;

    bool _isMarking;
    float _currentCooldown = 0;

    protected override void Start()
    {
        base.Start();
        _trail = GetTrail();
    }

    SkillTrail GetTrail()
    {
        return GetComponentInChildren<SkillTrail>();
    }

    protected override void CheckInput()
    {
        if (_currentCooldown > 0) _currentCooldown -= Time.deltaTime;
        else if (control.DefensiveSkill() && !_me.IsStunned && !_me.IsDisarmed)
        {
            _trail.ShowTrails();

            var blinkPos = _me.transform.position + _me.transform.forward * blinkDistance;

            if (_me.movDir != Vector3.zero)
            {
                blinkPos = _me.transform.position + _me.movDir.normalized * blinkDistance;
            }

            _currentCooldown = maxCooldown;

            StartCoroutine(TeleportTo(blinkPos));
        }
    }

    IEnumerator TeleportTo(Vector3 pos)
    {
        _me.GetRigidbody.MovePosition(pos);

        yield return new WaitForSeconds(.3f);

        _trail.StopShowing();

    }
}
