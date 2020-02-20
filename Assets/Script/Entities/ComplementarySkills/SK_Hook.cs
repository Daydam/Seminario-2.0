using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class SK_Hook : ComplementarySkillBase
{
    public SO_Hook skillData;

    readonly float _debugMaxTimeOfCast = 2f;

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
        _particleModule = GetComponent<ModuleParticleController>();

        skillData = Resources.Load<SO_Hook>("Scriptable Objects/Skills/Complementary/" + _owner.weightModule.prefix + GetSkillName() + _owner.weightModule.sufix) as SO_Hook;

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
        _owner.ApplyCastState(skillData.castTime);
        _particleModule.OnShoot();
        StartCoroutine(WaitForCastEnd(_owner.FinishedCasting));
    }

    public IEnumerator WaitForCastEnd(Func<bool> callback)
    {
        yield return new WaitUntil(callback);
        _currentCooldown = skillData.maxCooldown;
        NotifyUIModule();
        _canTap = false;

        StartCoroutine(LaunchHook());
    }

    IEnumerator LaunchHook()
    {
        
        _hook.gameObject.SetActive(true);
        _hook.Spawn(_owner.transform.position, _owner.transform.forward, _owner.tag, _owner, skillData);

        yield return new WaitUntil(() => _hook.movementFinished);

        yield return new WaitForSeconds(skillData.latchDelay);

        if (_hook.Target is Player)
        {
            var targetPlayer = (Player)_hook.Target;

            HeavierWeightBehaviour(targetPlayer);

            /*if (_owner.Weight > targetPlayer.Weight) HeavierWeightBehaviour(targetPlayer);
            else if (_owner.Weight < targetPlayer.Weight) LighterWeightBehaviour(targetPlayer.gameObject);
            else SameWeightBehaviour(targetPlayer);*/
        }
        else if (_hook.Target is DestructibleBase)
        {
            var tgt = (DestructibleBase)_hook.Target;
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
            _owner.ApplyStun(skillData.stunIfNullTarget);
        }

        _hook.gameObject.SetActive(false);
    }

    void SameWeightBehaviour(Player target)
    {
        var targetPosition = target.transform.position;
        var myPosition = _owner.transform.position;

        var centerPoint = Vector3.Lerp(myPosition, targetPosition, .5f);

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

        var otherLandingPoint = Vector3.Lerp(targetPosition, myPosition, .75f);

        StartCoroutine(HeavierWeightMovement(target, otherLandingPoint));
    }

    IEnumerator SameWeightMovement(Player player, Vector3 landingPoint)
    {
        player.CancelForces();
        var endCast = false;

        var debugTime = 0f;

        if (player.Equals(_owner)) player.ApplyCastState(() => endCast);
        else player.ApplyStun(() => endCast);

        var startPoint = player.transform.position;

        var tTick = Time.fixedDeltaTime / skillData.playerTravelTime;
        var elapsed = 0f;

        while (Vector3.Distance(player.transform.position, landingPoint) > .3f)
        {
            elapsed += tTick;

            var nextPos = Vector3.Lerp(startPoint, landingPoint, elapsed);

            player.GetRigidbody.MovePosition(nextPos);

            yield return new WaitForFixedUpdate();

            debugTime += Time.fixedDeltaTime;
            if (debugTime >= _debugMaxTimeOfCast)
            {
                endCast = true;
                _skillActive = false;
                _canTap = false;
                yield break;
            }
        }

        endCast = true;
        _skillActive = false;
        _canTap = false;
    }

    IEnumerator LighterWeightMovement(Player target, Vector3 landingPoint)
    {
        _owner.CancelForces();
        var endCast = false;

        var debugTime = 0f;

        if (target != null)
        {
            target.CancelForces();
            target.ApplyStun(() => endCast);
        }

        _owner.ApplyCastState(() => endCast);

        var startPoint = _owner.transform.position;

        var tTick = Time.fixedDeltaTime / skillData.playerTravelTime;
        var elapsed = 0f;

        while (Vector3.Distance(_owner.transform.position, landingPoint) > .3f)
        {
            elapsed += tTick;

            var nextPos = Vector3.Lerp(startPoint, landingPoint, elapsed);

            _owner.GetRigidbody.MovePosition(nextPos);

            yield return new WaitForFixedUpdate();

            debugTime += Time.fixedDeltaTime;
            if (debugTime >= _debugMaxTimeOfCast)
            {
                endCast = true;
                _skillActive = false;
                _canTap = false;
                yield break;
            }
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

        var debugTime = 0f;

        _owner.ApplyCastState(() => endCast);
        target.ApplyStun(() => endCast);

        var startPoint = target.transform.position;

        var tTick = Time.fixedDeltaTime / skillData.playerTravelTime;
        var elapsed = 0f;

        while (Vector3.Distance(target.transform.position, landingPoint) > .3f)
        {
            elapsed += tTick;

            var nextPos = Vector3.Lerp(startPoint, landingPoint, elapsed);

            target.GetRigidbody.MovePosition(nextPos);

            yield return new WaitForFixedUpdate();

            debugTime += Time.fixedDeltaTime;
            if (debugTime >= _debugMaxTimeOfCast)
            {
                endCast = true;
                _skillActive = false;
                _canTap = false;
                yield break;
            }
        }

        endCast = true;
        _skillActive = false;
        _canTap = false;
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

    public override float GetCooldownPerc()
    {
        return Mathf.Lerp(1, 0, _currentCooldown / skillData.maxCooldown);
    }
}
