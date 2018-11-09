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
    public AudioClip startVortex, endVortex;

    Renderer[] _rends;

    public float maxCooldown;
    public float blinkDistance;
    public float blinkDuration;
    public float disableDuration;

    readonly string _vertexShaderTag = "VertexCollapse";

    SkillTrail _trail;
    bool _canTap = true;
    float _currentCooldown = 0;

    protected override void Start()
    {
        base.Start();
        _rends = _owner.GetComponentsInChildren<Renderer>().Where(x => x.materials.Where(y => y.GetTag(_vertexShaderTag, true, "Nothing") == "true").Any()).ToArray();
        foreach (var item in _rends)
        {
            foreach (var mat in item.materials)
            {
                mat.SetVector("_CollapsePosition", new Vector3(55555, 55555,55555));
            }
        }
    }

    protected override void InitializeUseCondition()
    {
        _canUseSkill = () => !_owner.IsStunned && !_owner.IsDisarmed && !_owner.IsCasting && !_owner.lockedByGame && _currentCooldown <= 0;
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
                    //_stateSource.PlayOneShot(unavailableSound);
                }
            }
        }
        else _canTap = true;
    }

    IEnumerator TeleportHandler(Vector3 pos)
    {
        if (_rends == null)
        {
            _rends = _owner.GetComponentsInChildren<Renderer>().Where(x => x.materials.Where(y => y.GetTag(_vertexShaderTag, true, "Nothing") == "true").Any()).ToArray();
        }

        bool mid = false;

        _owner.ApplyCastState(blinkDuration + disableDuration);
        _owner.ApplyInvulnerability(blinkDuration);
        _stateSource.PlayOneShot(startVortex);

        var dir = pos - _owner.transform.position;

        var collapsePoint = Vector3.Lerp(_owner.GetRigidbody.position, pos, .5f);
        EventManager.Instance.DispatchEvent(Events.SkillEvents.VortexStart, _owner);

        foreach (var item in _rends)
        {
            foreach (var mat in item.materials)
            {
                mat.SetVector("_CollapsePosition", collapsePoint);
            }
        }

        _owner.GetRigidbody.isKinematic = true;

        var distanceTraveled = 0f;

        var amountByDelta = Time.fixedDeltaTime * blinkDistance / blinkDuration;

        while (distanceTraveled <= blinkDistance)
        {
            if (Mathf.Abs(distanceTraveled / blinkDistance) - 0.5f <= Mathf.Epsilon && !mid)
            {
                _stateSource.PlayOneShot(endVortex);
                mid = true;
            }

            distanceTraveled += amountByDelta;
            var nextPos = _owner.transform.position + amountByDelta * dir.normalized;
            _owner.GetRigidbody.MovePosition(nextPos);

            yield return new WaitForFixedUpdate();
        }

        _owner.GetRigidbody.isKinematic = false;

        foreach (var item in _rends)
        {
            foreach (var mat in item.materials)
            {
                mat.SetVector("_CollapsePosition", new Vector3(50000, 50000, 50000));
            }
        }

        StartCoroutine(WaitForCastEnd(_owner.FinishedCasting));
        EventManager.Instance.DispatchEvent(Events.SkillEvents.VortexEnd, _owner);
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
        foreach (var item in _rends)
        {
            item.material.SetVector("_CollapsePosition", new Vector3(50000, 50000, 50000));
        }
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
