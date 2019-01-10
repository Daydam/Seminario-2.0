using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SK_CruiserFlight : DefensiveSkillBase
{
    public SO_CruiserFlight skillData;

    bool _canTap = true, _skillActive = false;

    float _currentCooldown = 0, _startHeight;

    DMM_CruiserRepulsion _repulsion;

    protected override void Start()
    {
        base.Start();
        _startHeight = _owner.transform.position.y;

        skillData = Resources.Load<SO_CruiserFlight>("Scriptable Objects/Skills/Defensive/" + _owner.weightModule.prefix + GetSkillName() + _owner.weightModule.sufix) as SO_CruiserFlight;


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
        _owner.ApplyCastState(skillData.castTime);
        _skillActive = true;
        _owner.GetRigidbody.useGravity = false;
        _owner.GetRigidbody.isKinematic = true;
        _owner.CancelForces();

        var tTick = Time.fixedDeltaTime / skillData.castTime;
        var t = 0f;

        var startYCoord = _owner.transform.position.y;

        while (_owner.transform.position.y < skillData.height)
        {
            t += tTick;

            var nextYCoord = Mathf.Lerp(startYCoord, skillData.height, t);
            var nextPos = new Vector3(_owner.transform.position.x, nextYCoord, _owner.transform.position.z);

            _owner.GetRigidbody.MovePosition(nextPos);

            yield return new WaitForFixedUpdate();
        }
        _owner.GetRigidbody.MovePosition(new Vector3(_owner.transform.position.x, skillData.height, _owner.transform.position.z));

        StartCoroutine(InFlight());
    }

    IEnumerator InFlight()
    {
        var elapsed = 0f;
        _owner.ApplySlowMovement(skillData.airTime, skillData.speedMultiplier);
        _owner.ApplyDisarm(skillData.airTime);

        while (elapsed < skillData.airTime)
        {
            elapsed += Time.fixedDeltaTime;

            if (!Mathf.Approximately(_owner.transform.position.y, skillData.height))
                _owner.GetRigidbody.MovePosition(new Vector3(_owner.transform.position.x, skillData.height, _owner.transform.position.z));

            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(EndFlight());
    }

    IEnumerator EndFlight()
    {
        _owner.ApplyCastState(skillData.fallTime);

        var tTick = Time.fixedDeltaTime / skillData.fallTime;
        var t = 0f;

        while (_owner.transform.position.y > _startHeight)
        {
            t += tTick;

            var nextYCoord = Mathf.Lerp(skillData.height, _startHeight, t);
            var nextPos = new Vector3(_owner.transform.position.x, nextYCoord, _owner.transform.position.z);

            _owner.GetRigidbody.MovePosition(nextPos);

            yield return new WaitForFixedUpdate();
        }

        _owner.GetRigidbody.MovePosition(new Vector3(_owner.transform.position.x, _startHeight, _owner.transform.position.z));
        _repulsion.Spawn(_owner.transform.position, _owner, skillData);

        StartCoroutine(StunPostLanding());
    }

    IEnumerator StunPostLanding()
    {
        _owner.ApplyStun(skillData.stunPostLanding);
        _owner.GetRigidbody.isKinematic = false;
        _owner.GetRigidbody.useGravity = true;
        _owner.GetRigidbody.velocity = new Vector3(_owner.GetRigidbody.velocity.x, 0, _owner.GetRigidbody.velocity.z);

        yield return new WaitForSeconds(skillData.stunPostLanding);

        _currentCooldown = skillData.maxCooldown;
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
