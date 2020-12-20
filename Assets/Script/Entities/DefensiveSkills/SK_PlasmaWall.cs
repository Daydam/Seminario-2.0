using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SK_PlasmaWall : DefensiveSkillBase
{
    public SO_PlasmaWall skillData;
    public AudioClip spawnSound;

    Transform spawnPos;

    float _currentCooldown = 0;
    bool _canTap = true;

    public Transform SpawnPos
    {
        get
        {
            if (!spawnPos) spawnPos = transform.Find("SpawnPoint");
            return spawnPos;
        }
    }

    protected override void Start()
    {
        base.Start();
        skillData = Resources.Load<SO_PlasmaWall>("Scriptable Objects/Skills/Defensive/" + _owner.weightModule.prefix + GetSkillName() + _owner.weightModule.sufix) as SO_PlasmaWall;
    }

    protected override void InitializeUseCondition()
    {
        _canUseSkill = () => !_owner.IsStunned && !_owner.IsDisarmed && !_owner.IsCasting && !_owner.lockedByGame && _currentCooldown <= 0;
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
                    SpawnWall();
                    _currentCooldown = skillData.maxCooldown;
                    NotifyUIModule();
                }
            }
            else
            {
                if (_canTap)
                {
                    _canTap = false;
                }
            }
        }
        else _canTap = true;
    }

    void SpawnWall()
    {
        PlasmaWallSpawner.Instance.ObjectPool.GetObjectFromPool().Spawn(SpawnPos.position, _owner.transform.forward, skillData.size, skillData);
        _stateSource.PlayOneShot(spawnSound,2.5f);
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
