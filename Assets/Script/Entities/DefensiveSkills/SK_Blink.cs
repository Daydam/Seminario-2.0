using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SK_Blink : DefensiveSkillBase
{
    public float maxCooldown;
    public float blinkDistance;
    SkillTrail _trail;

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
        else if (control.DefensiveSkill() && !_owner.IsStunned && !_owner.IsDisarmed)
        {
            _trail.ShowTrails();

            var blinkPos = _owner.transform.position + _owner.transform.forward * blinkDistance;

            if (_owner.movDir != Vector3.zero)
            {
                blinkPos = _owner.transform.position + _owner.movDir.normalized * blinkDistance;
            }

            _currentCooldown = maxCooldown;

            StartCoroutine(TeleportTo(blinkPos));
        }
    }

    IEnumerator TeleportTo(Vector3 pos)
    {
        _owner.GetRigidbody.MovePosition(pos);

        yield return new WaitForSeconds(.3f);

        _trail.StopShowing();

    }

    public override void ResetRound()
    {
        _currentCooldown = 0;
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
