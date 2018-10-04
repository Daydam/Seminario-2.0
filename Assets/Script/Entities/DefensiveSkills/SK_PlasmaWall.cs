using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SK_PlasmaWall : DefensiveSkillBase
{
    Transform spawnPos;
    public float maxCooldown = 6f;
    public int maxCharges = 3;

    int _actualCharges;
    float _currentCooldown = 0;
    bool _canUse = true;

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

        _actualCharges = maxCharges;
    }

    protected override void Update()
    {
        CheckForCharges();
        base.Update();
    }

    void CheckForCharges()
    {
        if (_actualCharges < maxCharges)
        {
            if (_currentCooldown > 0) _currentCooldown -= Time.deltaTime;
            else
            {
                _actualCharges++;
                _currentCooldown = maxCooldown;
            }
        }
    }

    protected override void CheckInput()
    {
        if (control.DefensiveSkill())
        {
            if (_actualCharges > 0 && !_owner.IsStunned && !_owner.IsDisarmed && _canUse && !_owner.IsCasting)
            {
                _actualCharges--;
                _canUse = false;
                SpawnWall();
            }
        }
        else _canUse = true;
    }

    void SpawnWall()
    {
        PlasmaWallSpawner.Instance.ObjectPool.GetObjectFromPool().Spawn(SpawnPos.position, _owner.transform.forward);
    }

    public override void ResetRound()
    {
        _actualCharges = maxCharges;
        _currentCooldown = 0;
        _canUse = true;
    }

    public override SkillState GetActualState()
    {
        var unavailable = _actualCharges <= 0;
        var reloading = _actualCharges > 0 && _actualCharges < maxCharges;
        var userDisabled = _owner.IsStunned || _owner.IsDisarmed;

        if (userDisabled) return SkillState.UserDisabled;
        else if (unavailable) return SkillState.Unavailable;
        else if (reloading) return SkillState.Reloading;
        else return SkillState.Available;
    }
}
