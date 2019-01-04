using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SK_Hook : ComplementarySkillBase
{
    public float playerTravelTime = .6f;
    public float stunIfNullTarget = .4f;
    public float castTime = .2f;
    public float maxCooldown = 6;

    bool _skillActive = false, _canTap = true;
    float _currentCooldown = 0;

    float GetDistanceToLandingPoint(float myWeight, float otherWeight)
    {
        return Mathf.Approximately(myWeight, otherWeight) ? .5f : 1f;
    }

    DMM_Hook _hook;

    protected override void Start()
    {
        base.Start();

        var loadedPrefab = Resources.Load<DMM_Hook>("Prefabs/Projectiles/Hook");

        _hook = Instantiate(loadedPrefab);
        _hook.gameObject.SetActive(false);
    }

    protected override void InitializeUseCondition()
    {
        _canUseSkill = () => !_owner.IsStunned && !_owner.IsDisarmed && !_owner.IsCasting && !_skillActive && !_owner.lockedByGame && _currentCooldown <= 0;
    }

    protected override void CheckInput()
    {
        if (_currentCooldown > 0) _currentCooldown -= Time.deltaTime;

        if (inputMethod())
        {
            if (_canUseSkill())
            {
                if (_canTap)
                {
                    _canTap = false;
                    UseSkill();
                }
            }
            //else _stateSource.PlayOneShot(unavailableSound);
        }
        else _canTap = true;
    }

    void UseSkill()
    {
        _owner.ApplyCastState(castTime);

        StartCoroutine(WaitForCastEnd(_owner.FinishedCasting));
    }

    public IEnumerator WaitForCastEnd(Func<bool> callback)
    {
        yield return new WaitUntil(callback);
        _currentCooldown = maxCooldown;
        _canTap = false;

        StartCoroutine(LaunchHook());
    }

    IEnumerator LaunchHook()
    {
        _hook.gameObject.SetActive(true);
        _hook.Spawn(_owner.transform.position, _owner.transform.forward, _owner.tag, _owner);

        Func<bool> hookCallback = () => _hook.movementFinished;
        yield return new WaitUntil(hookCallback);

        if (_hook.Target is Player)
        {
            //weight test
        }
        else if (_hook.Target is RingWall)
        {
            var tgt = (RingWall)_hook.Target;
            LighterWeightMovement(tgt.gameObject, /*_owner.Weight*/ GetDistanceToLandingPoint(0, float.MaxValue), () => { });
        }
        else if (_hook.Target is RingStructure)
        {
            var tgt = (RingStructure)_hook.Target;
            LighterWeightMovement(tgt.gameObject, /*_owner.Weight*/ GetDistanceToLandingPoint(0, float.MaxValue), () => { });
        }
        else if (_hook.Target is DMM_PlasmaWall)
        {
            var tgt = (DMM_PlasmaWall)_hook.Target;
            LighterWeightMovement(tgt.gameObject, /*_owner.Weight*/ GetDistanceToLandingPoint(0, float.MaxValue), () => { });
        }
        else
        {
            _owner.ApplyStun(stunIfNullTarget);
        }

        _hook.gameObject.SetActive(false);
    }

    void SameWeightMovement(Player target, float displacementDistance, Action<Vector3> enemyLandingPointCallback)
    {
        var centerPoint = Vector3.Lerp(transform.position, target.transform.position, .5f);

        var myDisplacementDir = (centerPoint - transform.position).normalized * displacementDistance;
        var myLandingPoint = centerPoint - myDisplacementDir;

        var otherDisplacementDir = (centerPoint - target.transform.position).normalized * displacementDistance;
        var otherLandingPoint = centerPoint - otherDisplacementDir;

        enemyLandingPointCallback(otherLandingPoint);

        StartCoroutine(MoveToLandingPosition(myLandingPoint, Vector3.Distance(transform.position, myLandingPoint)));
    }

    void LighterWeightMovement(GameObject target, float displacementDistance, Action enemyCallback)
    {
        //var myDisplacementDir = (transform.position - target.transform.position).normalized * displacementDistance;
        var myDisplacementDir = (target.transform.position - transform.position).normalized * displacementDistance;
        var myLandingPoint = target.transform.position - myDisplacementDir;

        enemyCallback();

        StartCoroutine(MoveToLandingPosition(myLandingPoint, Vector3.Distance(transform.position, myLandingPoint)));
    }

    void HeavierWeightMovement(Player target, float displacementDistance, Action<Vector3> enemyLandingPointCallback)
    {
        //var enemyDisplacementDir = (target.transform.position - transform.position).normalized * displacementDistance;
        var enemyDisplacementDir = (transform.position - target.transform.position).normalized * displacementDistance;
        var enemyLandingPoint = transform.position - enemyDisplacementDir;

        enemyLandingPointCallback(enemyLandingPoint);

        //StartCoroutine(WaitForEnemyToArrive(target.ArrivedOnHookCallback));
    }

    IEnumerator MoveToLandingPosition(Vector3 landingPoint, float distanceToLandingPoint)
    {
        _owner.CancelForces();

        var distanceTraveled = 0f;

        var amountByDelta = Time.fixedDeltaTime * distanceToLandingPoint / playerTravelTime;
        var dir = landingPoint - transform.position;

        while (distanceTraveled < distanceToLandingPoint)
        {
            distanceTraveled += amountByDelta;
            var nextPos = _owner.GetRigidbody.position + amountByDelta * dir.normalized;
            _owner.GetRigidbody.MovePosition(nextPos);

            yield return new WaitForFixedUpdate();
        }

        _skillActive = false;
        _canTap = false;
    }

    IEnumerator WaitForEnemyToArrive(Func<bool> enemyArriveCallback)
    {
        yield return new WaitUntil(enemyArriveCallback);

        _skillActive = false;
        _canTap = false;
    }

    public override void ResetRound()
    {
        StopAllCoroutines();
        _currentCooldown = 0;
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
