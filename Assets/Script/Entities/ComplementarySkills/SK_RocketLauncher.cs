using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PhoenixDevelopment;

public class SK_RocketLauncher : ComplementarySkillBase
{
    public SO_RocketLauncher skillData;

    bool _canTap = true;
    float _currentCooldown = 0;

    protected override void InitializeUseCondition()
    {
        _canUseSkill = () => !_owner.IsStunned && !_owner.IsDisarmed && !_owner.IsCasting && !_owner.lockedByGame && _currentCooldown <= 0;
    }

    protected override void Start()
    {
        base.Start();
        skillData = Resources.Load<SO_RocketLauncher>("Scriptable Objects/Skills/Complementary/" + _owner.weightModule.prefix + GetSkillName() + _owner.weightModule.sufix) as SO_RocketLauncher;
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
                    if (_activationAnim != null) _activationAnim.Play();
                    ShootProjectile();
                    _currentCooldown = skillData.maxCooldown;
                    NotifyUIModule();
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

        if (Physics.Raycast(_owner.transform.position, _owner.gameObject.transform.forward, out rch, skillData.maxRange, maskOfLayers))
        {
            dir = rch.collider.transform.position - transform.position;
        }
        else dir = _owner.gameObject.transform.forward;

        FragmentMissileSpawner.Instance.ObjectPool.GetObjectFromPool().Spawn(transform.position, dir, skillData.maxRange, _owner.gameObject.tag, _owner, skillData);
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

    public override float GetCooldownPerc()
    {
        return Mathf.Lerp(1, 0, _currentCooldown / skillData.maxCooldown);
    }
}
