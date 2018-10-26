﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SK_FragmentMissile : ComplementarySkillBase
{
    public float maxCooldown;
    public float minRange, maxRange;

    bool _canTap = true;
    float _currentCooldown = 0;

    protected override void InitializeUseCondition()
    {
        _canUseSkill = () => !_owner.IsStunned && !_owner.IsDisarmed && !_owner.IsCasting && !_owner.lockedByGame && _currentCooldown <= 0;
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
                    if (activationAnim != null) activationAnim.Play();
                    ShootProjectile();
                    _currentCooldown = maxCooldown;
                }
            }
            //else _stateSource.PlayOneShot(unavailableSound);
        }
        else _canTap = true;
    }

    void ShootProjectile()
    {
        var otherPlayerLayers = new int[] { LayerMask.NameToLayer("Player1"), LayerMask.NameToLayer("Player2"), LayerMask.NameToLayer("Player3"), LayerMask.NameToLayer("Player4") }
                                            .Where(x => x != _owner.gameObject.layer)
                                            .ToArray();

        var maskOfLayers = 0.MutateTo(otherPlayerLayers);

        RaycastHit rch;
        Vector3 dir;

        if (Physics.Raycast(_owner.transform.position, _owner.gameObject.transform.forward, out rch, maxRange, maskOfLayers))
        {
            dir = rch.collider.transform.position - transform.position;
        }
        else dir = _owner.gameObject.transform.forward;

        FragmentMissileSpawner.Instance.ObjectPool.GetObjectFromPool().Spawn(transform.position, dir, maxRange, _owner.gameObject.tag, _owner);
    }

    public override void ResetRound()
    {
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
