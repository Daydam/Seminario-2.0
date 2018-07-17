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
    bool _canUse = true;
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
        if (control.DefensiveSkill())
        {
            if (_actualCharges > 0 && !_owner.IsStunned && !_owner.IsDisarmed && !_isDashing && _canUse)
            {
                _trail.ShowTrails();

                var dirV = _owner.transform.forward;

                if (_owner.movDir != Vector3.zero)
                {
                    dirV = _owner.movDir;
                }

                _actualCharges--;
                _canUse = false;
                StartCoroutine(DashHandler(dirV.normalized));
            }
        }
        else _canUse = true;
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
            var nextPos = _owner.GetRigidbody.position + amountByDelta * dir;
            _owner.GetRigidbody.MovePosition(nextPos);

            yield return new WaitForFixedUpdate();
        }

        _trail.StopShowing();

        _isDashing = false;
    }

    public override void ResetRound()
    {
        _actualCharges = maxCharges;
        _currentCooldown = 0;
        _canUse = true;
        _isDashing = false;
        _trail.StopShowing();
    }

    public override SkillState GetActualState()
    {
        var unavailable = _actualCharges <= 0;
        var reloading = _actualCharges < 0 && _actualCharges < maxCharges;
        var userDisabled = _owner.IsStunned || _owner.IsDisarmed;

        if (userDisabled) return SkillState.UserDisabled;
        else if (unavailable) return SkillState.Unavailable;
        else if (reloading) return SkillState.Reloading;
        else return SkillState.Available;
    }
}
