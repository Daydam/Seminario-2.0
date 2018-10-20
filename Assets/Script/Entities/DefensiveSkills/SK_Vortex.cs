using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// El favorito de papi :)
/// </summary>
public class SK_Vortex : DefensiveSkillBase
{
    Renderer[] _rends;

    public Shader[] acceptedShaders;

    public float maxCooldown;
    public float blinkDistance;
    public float blinkDuration;
    public float disableDuration;

    SkillTrail _trail;
    bool _canTap = true;
    float _currentCooldown = 0;

    protected override void Start()
    {
        base.Start();

        _rends = _owner.GetComponentsInChildren<Renderer>()
            .Aggregate(FList.Create<Renderer>(), (acum, curr) =>
            {
                for (int i = 0; i < acceptedShaders.Length; i++)
                {
                    if (curr.material.shader == acceptedShaders[i]) acum += curr;
                }
                return acum;
            }).ToArray();
    }

    protected override void InitializeUseCondition()
    {
        _canUseSkill = () => !_owner.IsStunned && !_owner.IsDisarmed && !_owner.IsCasting && _currentCooldown <= 0;
    }

    SkillTrail GetTrail()
    {
        return GetComponentInChildren<SkillTrail>();
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
                    _canTap = false;
                    var blinkPos = _owner.transform.position + _owner.transform.forward * blinkDistance;
                    var partDir = _owner.transform.forward;

                    if (_owner.movDir != Vector3.zero)
                    {
                        blinkPos = _owner.transform.position + _owner.movDir.normalized * blinkDistance;
                        partDir = _owner.movDir.normalized;
                    }

                    _currentCooldown = maxCooldown;

                    SimpleParticleSpawner.Instance.SpawnParticle(SimpleParticleSpawner.ParticleID.VORTEX, _owner.transform.position, partDir);

                    StartCoroutine(TeleportHandler(blinkPos));
                }
            }
            else
            {
                if (_canTap)
                {
                    _canTap = false;
                    _stateSource.PlayOneShot(unavailableSound);
                }
            }
        }
        else _canTap = true;
    }

    IEnumerator TeleportHandler(Vector3 pos)
    {
        _owner.ApplyCastState(blinkDuration + disableDuration);
        _owner.ApplyInvulnerability(blinkDuration);

        var dir = pos - _owner.transform.position;

        var collapsePoint = Vector3.Lerp(_owner.GetRigidbody.position, pos, .5f);

        foreach (var item in _rends)
        {
            item.material.SetVector("_CollapsePosition", collapsePoint);
        }

        _owner.GetRigidbody.isKinematic = true;

        var distanceTraveled = 0f;

        var amountByDelta = Time.fixedDeltaTime * blinkDistance / blinkDuration;

        while (distanceTraveled <= blinkDistance)
        {
            distanceTraveled += amountByDelta;
            var nextPos = _owner.transform.position + amountByDelta * dir.normalized;
            _owner.GetRigidbody.MovePosition(nextPos);

            yield return new WaitForFixedUpdate();
        }

        _owner.GetRigidbody.isKinematic = false;

        foreach (var item in _rends)
        {
            item.material.SetVector("_CollapsePosition", new Vector3(500, 500, 500));
        }

        StartCoroutine(WaitForCastEnd(_owner.FinishedCasting));
    }

    public IEnumerator WaitForCastEnd(Func<bool> callback)
    {
        yield return new WaitUntil(callback);

        _currentCooldown = maxCooldown;
        _canTap = false;
    }

    public override void ResetRound()
    {
        StopAllCoroutines();
        _owner.GetRigidbody.isKinematic = false;
        _currentCooldown = 0;
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
