using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SK_Dash : DefensiveSkillBase
{
    public float maxCooldown;
    public float dashDistance;
    public float dashTime;
    SkillTrail _trail;

    bool _isDashing;
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

    protected override void InitializeUseCondition()
    {
        _canUseSkill = () => !_owner.IsStunned && !_owner.IsDisarmed && !_isDashing && !_owner.IsCasting;
    }

    protected override void CheckInput()
    {
        if (_currentCooldown > 0) _currentCooldown -= Time.deltaTime;
        else if (control.DefensiveSkill() && _canUseSkill())
        {
            _trail.ShowTrails();

            var dirV = _owner.transform.forward;

            if (_owner.movDir != Vector3.zero)
            {
                dirV = _owner.movDir;
            }

            StartCoroutine(DashHandler(dirV.normalized));
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
        _currentCooldown = 0;
        _isDashing = false;
        _trail.StopShowing();
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
