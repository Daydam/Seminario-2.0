using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SK_CruiserFlight : DefensiveSkillBase
{
    bool _canTap = true, _skillActive = false;

    float _currentCooldown = 0, _startHeight;
    float maxCooldown = 6, airTime = 1.5f, stunPostLanding = .3f, fallTime = .2f, height = 4, castTime = .2f;
    [Range(0, 1)]
    public float speedMultiplier = .75f;

    DMM_CruiserRepulsion _repulsion;

    protected override void Start()
    {
        base.Start();
        _startHeight = _owner.transform.position.y;
        var obj = Resources.Load<DMM_CruiserRepulsion>("Prefabs/Projectiles/CruiserRepulsion");
        _repulsion = Instantiate(obj, transform.position, Quaternion.identity);
        _repulsion.gameObject.SetActive(false);
    }

    protected override void InitializeUseCondition()
    {
        _canUseSkill = () => !_owner.IsStunned && !_owner.IsDisarmed && !_owner.IsCasting && !_owner.lockedByGame && !_skillActive && _currentCooldown <= 0;
    }

    protected override void CheckInput()
    {
        if (_currentCooldown > 0) _currentCooldown -= Time.deltaTime;

        if (control.DefensiveSkill())
        {
            if (_canUseSkill())
            {
                if (_canTap)
                {
                    StopAllCoroutines();
                    StartCoroutine(StartFlight());
                }
            }
            else
            {
                if (_canTap)
                {
                    _canTap = false;
                    //_stateSource.PlayOneShot(unavailableSound);
                }
            }
        }
        else _canTap = true;
    }

    IEnumerator StartFlight()
    {
        _owner.ApplyCastState(castTime);
        _skillActive = true;
        _owner.GetRigidbody.useGravity = false;
        _owner.GetRigidbody.isKinematic = true;
        _owner.CancelForces();

        var tTick = Time.fixedDeltaTime / castTime;
        var t = 0f;

        var startYCoord = _owner.transform.position.y;

        while (_owner.transform.position.y < height)
        {
            t += tTick;

            var nextYCoord = Mathf.Lerp(startYCoord, height, t);
            var nextPos = new Vector3(_owner.transform.position.x, nextYCoord, _owner.transform.position.z);

            _owner.GetRigidbody.MovePosition(nextPos);

            yield return new WaitForFixedUpdate();
        }
        _owner.GetRigidbody.MovePosition(new Vector3(_owner.transform.position.x, height, _owner.transform.position.z));

        StartCoroutine(InFlight());
    }

    IEnumerator InFlight()
    {
        var elapsed = 0f;
        _owner.ApplySlowMovement(airTime, speedMultiplier);
        _owner.ApplyDisarm(airTime);

        while (elapsed < airTime)
        {
            elapsed += Time.fixedDeltaTime;

            if (!Mathf.Approximately(_owner.transform.position.y, height)) _owner.GetRigidbody.MovePosition(new Vector3(_owner.transform.position.x, height, _owner.transform.position.z));

            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(EndFlight());
    }

    IEnumerator EndFlight()
    {
        _owner.ApplyCastState(fallTime);

        var tTick = Time.fixedDeltaTime / fallTime;
        var t = 0f;

        while (_owner.transform.position.y > _startHeight)
        {
            t += tTick;

            var nextYCoord = Mathf.Lerp(height, _startHeight, t);
            var nextPos = new Vector3(_owner.transform.position.x, nextYCoord, _owner.transform.position.z);

            _owner.GetRigidbody.MovePosition(nextPos);

            yield return new WaitForFixedUpdate();
        }

        _owner.GetRigidbody.MovePosition(new Vector3(_owner.transform.position.x, _startHeight, _owner.transform.position.z));
        _repulsion.Spawn(_owner.transform.position, _owner);

        StartCoroutine(StunPostLanding());
    }

    IEnumerator StunPostLanding()
    {
        _owner.ApplyStun(stunPostLanding);
        _owner.GetRigidbody.isKinematic = false;
        _owner.GetRigidbody.useGravity = true;
        _owner.GetRigidbody.velocity = new Vector3(_owner.GetRigidbody.velocity.x, 0, _owner.GetRigidbody.velocity.z);

        yield return new WaitForSeconds(stunPostLanding);

        _currentCooldown = maxCooldown;
        _skillActive = false;
    }

    public override void ResetRound()
    {
        StopAllCoroutines();
        _currentCooldown = 0;
        _skillActive = false;
    }

    public override SkillState GetActualState()
    {
        var unavailable = _currentCooldown > 0;
        var userDisabled = _owner.IsStunned || _owner.IsDisarmed;

        if (userDisabled) return SkillState.UserDisabled;
        else if (unavailable) return SkillState.Unavailable;
        else if (_skillActive) return SkillState.Active;
        else return SkillState.Available;
    }
}
