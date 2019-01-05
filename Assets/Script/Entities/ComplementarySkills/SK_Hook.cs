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

    public enum HookBehaviours { SAME, LIGHT, HEAVY };

    public HookBehaviours actual;

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

        yield return new WaitUntil(() => _hook.movementFinished);

        if (_hook.Target is Player)
        {
            var targetPlayer = (Player)_hook.Target;

            if (_owner.weight > targetPlayer.weight) HeavierWeightBehaviour(targetPlayer);
            else if (_owner.weight < targetPlayer.weight) LighterWeightBehaviour(targetPlayer.gameObject);
            else SameWeightBehaviour(targetPlayer);
        }
        else if (_hook.Target is RingWall)
        {
            var tgt = (RingWall)_hook.Target;
            LighterWeightBehaviour(tgt.gameObject);
        }
        else if (_hook.Target is RingStructure)
        {
            var tgt = (RingStructure)_hook.Target;
            LighterWeightBehaviour(tgt.gameObject);
        }
        else if (_hook.Target is DMM_PlasmaWall)
        {
            var tgt = (DMM_PlasmaWall)_hook.Target;
            LighterWeightBehaviour(tgt.gameObject);
        }
        else
        {
            _owner.ApplyStun(stunIfNullTarget);
        }

        _hook.gameObject.SetActive(false);
    }

    void SameWeightBehaviour(Player target)
    {
        var targetPosition = target.transform.position;
        var myPosition = _owner.transform.position;

        var centerPoint = Vector3.Lerp(myPosition, targetPosition, .75f);

        var myLandingPoint = Vector3.Lerp(myPosition, centerPoint, .75f);
        var otherLandingPoint = Vector3.Lerp(targetPosition, centerPoint, .75f);

        StartCoroutine(SameWeightMovement(_owner, myLandingPoint));
        StartCoroutine(SameWeightMovement(target, otherLandingPoint));
    }

    void LighterWeightBehaviour(GameObject target)
    {
        Vector3 targetPosition;

        if (target.GetComponent<Player>() != null) targetPosition = target.transform.position;
        else targetPosition = target.GetComponent<Collider>().ClosestPoint(_hook.transform.position);

        var myPosition = _owner.transform.position;

        var myLandingPoint = Vector3.Lerp(myPosition, targetPosition, .75f);

        StartCoroutine(LighterWeightMovement(target.GetComponent<Player>(), myLandingPoint));
    }

    void HeavierWeightBehaviour(Player target)
    {
        var targetPosition = target.transform.position;
        var myPosition = _owner.transform.position;

        var otherLandingPoint = Vector3.Lerp(targetPosition, myPosition, .55f);

        StartCoroutine(HeavierWeightMovement(target, otherLandingPoint));
    }

    IEnumerator SameWeightMovement(Player player, Vector3 landingPoint)
    {
        player.CancelForces();
        var endCast = false;

        if (player.Equals(_owner)) player.ApplyCastState(() => endCast);
        else player.ApplyStun(() => endCast);

        var startPoint = player.transform.position;

        var tTick = Time.fixedDeltaTime / playerTravelTime;
        var elapsed = 0f;

        while (Vector3.Distance(player.transform.position, landingPoint) > .3f)
        {
            elapsed += tTick;

            var nextPos = Vector3.Lerp(startPoint, landingPoint, elapsed);

            player.GetRigidbody.MovePosition(nextPos);

            yield return new WaitForFixedUpdate();
        }

        endCast = true;
        _skillActive = false;
        _canTap = false;
    }

    IEnumerator LighterWeightMovement(Player target, Vector3 landingPoint)
    {
        _owner.CancelForces();
        var endCast = false;

        if (target != null)
        {
            target.CancelForces();
            target.ApplyStun(() => endCast);
        }

        _owner.ApplyCastState(() => endCast);

        var startPoint = _owner.transform.position;

        var tTick = Time.fixedDeltaTime / playerTravelTime;
        var elapsed = 0f;

        while (Vector3.Distance(_owner.transform.position, landingPoint) > .3f)
        {
            elapsed += tTick;

            var nextPos = Vector3.Lerp(startPoint, landingPoint, elapsed);

            _owner.GetRigidbody.MovePosition(nextPos);

            yield return new WaitForFixedUpdate();
        }

        endCast = true;
        _skillActive = false;
        _canTap = false;
    }

    IEnumerator HeavierWeightMovement(Player target, Vector3 landingPoint)
    {
        _owner.CancelForces();
        target.CancelForces();

        var endCast = false;

        _owner.ApplyCastState(() => endCast);
        target.ApplyStun(() => endCast);

        var startPoint = target.transform.position;

        var tTick = Time.fixedDeltaTime / playerTravelTime;
        var elapsed = 0f;

        while (Vector3.Distance(target.transform.position, landingPoint) > .3f)
        {
            elapsed += tTick;

            var nextPos = Vector3.Lerp(startPoint, landingPoint, elapsed);

            target.GetRigidbody.MovePosition(nextPos);

            yield return new WaitForFixedUpdate();
        }

        endCast = true;
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
