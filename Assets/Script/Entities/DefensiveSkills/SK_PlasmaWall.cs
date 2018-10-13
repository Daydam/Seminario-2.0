using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SK_PlasmaWall : DefensiveSkillBase
{
    Transform spawnPos;
    public float maxCooldown = 6f;

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

    protected override void InitializeUseCondition()
    {
        _canUseSkill = () => !_owner.IsStunned && !_owner.IsDisarmed && !_owner.IsCasting;
    }

    protected override void CheckInput()
    {
        if (_currentCooldown > 0) _currentCooldown -= Time.deltaTime;

        if (control.DefensiveSkill() && _canUseSkill())
        {
            if (_canTap && _currentCooldown <= 0)
            {
                _canTap = false;
                SpawnWall();
                _currentCooldown = maxCooldown;
            }
        }
        else _canTap = true;
    }

    void SpawnWall()
    {
        PlasmaWallSpawner.Instance.ObjectPool.GetObjectFromPool().Spawn(SpawnPos.position, _owner.transform.forward);
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
