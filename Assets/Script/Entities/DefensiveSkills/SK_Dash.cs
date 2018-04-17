using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SK_Dash : DefensiveSkillBase
{
    public float maxCooldown;
    public int maxCharges;
    public float dashDistance;
    public float dashTime;
    SkillTrail _trail;

    bool _isDashing;
    int _actualCharges;
    float _currentCooldown = 0;

    protected override void Start()
    {
        base.Start();
        _actualCharges = maxCharges;
        _trail = GetTrail();
    }

    SkillTrail GetTrail()
    {
        return GetComponentInChildren<SkillTrail>();
    }

    protected override void Update()
    {
        CheckForCharges();
        base.Update();
    }

    protected override void CheckInput()
    {
        if (control.DefensiveSkill() && _actualCharges > 0 && !_isDashing && !_me.IsStunned && !_me.IsDisarmed)
        {
            _trail.ShowTrails();

            var dirV = _me.transform.forward;

            if (_me.movDir != Vector3.zero)
            {
                dirV = _me.movDir;
            }

            _actualCharges--;
            StartCoroutine(DashHandler(dirV.normalized));
        }
        
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

    IEnumerator DashHandler(Vector3 dir)
    {
        _isDashing = true;

        var distanceTraveled = 0f;

        var amountByDelta = Time.fixedDeltaTime * dashDistance / dashTime;

        while (distanceTraveled < dashDistance)
        {
            distanceTraveled += amountByDelta;
            var nextPos = _me.GetRigidbody.position + amountByDelta * dir;
            _me.GetRigidbody.MovePosition(nextPos);

            yield return new WaitForFixedUpdate();
        }

        _trail.StopShowing();

        _isDashing = false;
    }
}
