using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PhoenixDevelopment;

public class SK_StunMissile : ComplementarySkillBase
{
    public SO_StunMissile skillData;
    public AudioClip soundLaunch;
    [HideInInspector]
    public AudioSource audioSource;

    bool _canTap = true;
    float _currentCooldown = 0;

    protected override void Start()
    {
        base.Start();
        audioSource = GetComponent<AudioSource>();
        _particleModule = GetComponent<ModuleParticleController>();
        skillData = Resources.Load<SO_StunMissile>("Scriptable Objects/Skills/Complementary/" + _owner.weightModule.prefix + GetSkillName() + _owner.weightModule.sufix) as SO_StunMissile;
    }

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
                    if (_activationAnim != null) _activationAnim.Play();
                    _canTap = false;
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
        audioSource.PlayOneShot(soundLaunch,2f);
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

        _particleModule.OnShoot();
        StartCoroutine(ApplyReadyParticles());

        StunMissileSpawner.Instance.ObjectPool.GetObjectFromPool().Spawn(transform.position, dir, _owner, skillData.maxRange, _owner.gameObject.tag, skillData);
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
        else return SkillState.Available;
    }

    public override float GetCooldownPerc()
    {
        return Mathf.Lerp(1, 0, _currentCooldown / skillData.maxCooldown);
    }

    public IEnumerator ApplyReadyParticles()
    {
        yield return new WaitForEndOfFrame();
        while (true)
        {
            if (GetActualState() != SkillState.Available)
            {
                yield return new WaitForEndOfFrame();
            }
            else
            {
                _particleModule.OnAvailableShot();
                yield break;
            }
        }
    }
}
