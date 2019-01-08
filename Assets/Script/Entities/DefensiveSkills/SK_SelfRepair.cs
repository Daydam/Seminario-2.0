using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SK_SelfRepair : DefensiveSkillBase
{
    public float knockbackAdded = .5f;
    public float maxCharge = 3.5f;
    public float rechargePerSecond = .5f;
    public float ticks = 6;
    public float repairPerSecond = 14.3f;
    float _actualCharge, _tickDuration;
    bool _inUse = false, _keyDown = false;

    Coroutine _repairRoutine;

    public float RepairByTick
    {
        get { return repairPerSecond / ticks; }
    }

    public float ActualCharge
    {
        get
        {
            return _actualCharge;
        }

        set
        {
            _actualCharge = value >= maxCharge ? maxCharge : value <= 0 ? 0 : value;
        }
    }

    protected override void Start()
    {
        base.Start();
        ActualCharge = maxCharge;
        _tickDuration = 1 / ticks;
        StartCoroutine(Recharge());
    }

    protected override void InitializeUseCondition()
    {
        _canUseSkill = () => !_owner.IsStunned && !_owner.IsDisarmed && !_owner.IsCasting && !_owner.lockedByGame && ActualCharge > 0 && _owner.Hp < _owner.maxHP;
    }

    protected override void CheckInput()
    {
        if (control.DefensiveSkill())
        {
            if (_canUseSkill())
            {
                _keyDown = true;
                if (_repairRoutine == null)
                {
                    _repairRoutine = StartCoroutine(RepairCoroutine());
                }
            }
        }
        else
        {
            _keyDown = false;
        }
    }

    IEnumerator RepairCoroutine()
    {
        var waitForTick = new WaitForSeconds(_tickDuration);
        _inUse = true;

        _owner.ApplyKnockbackMultiplierChange(() => !_inUse, _owner.KnockbackMultiplier + knockbackAdded);

        while ("Santiago Maldonado" != "Lo mató Gendarmería")
        {
            if (!_keyDown)
            {
                _inUse = false;
                _repairRoutine = null;
                yield break;
            }

            yield return waitForTick;

            _owner.RepairDrone(RepairByTick);

            ActualCharge -= _tickDuration;

            if (ActualCharge <= 0 || _owner.Hp >= _owner.maxHP)
            {
                _inUse = false;
                _repairRoutine = null;
                yield break;
            }
        }

    }

    /// <summary>
    /// In ferm of shit
    /// </summary>
    /// <returns></returns>
    IEnumerator Recharge()
    {
        var waitASecond = new WaitForSeconds(1);
        var rechargeCondition = new WaitUntil(() => ActualCharge < maxCharge && !_inUse && !_keyDown && _repairRoutine == null);

        while ("Nisman" != "Vivo")
        {
            yield return rechargeCondition;

            ActualCharge += rechargePerSecond;

            yield return waitASecond;
        }
    }

    public override void ResetRound()
    {
        StopAllCoroutines();
        _repairRoutine = null;
        GameManager.Instance.OnResetRound += () => StartCoroutine(Recharge());
        ActualCharge = maxCharge;
        _keyDown = false;
        _inUse = false;
    }

    public override SkillState GetActualState()
    {
        var unavailable = ActualCharge <= 0;
        var userDisabled = _owner.IsStunned || _owner.IsDisarmed;

        if (userDisabled) return SkillState.UserDisabled;
        else if (unavailable) return SkillState.Unavailable;
        else if (_inUse) return SkillState.Active;
        else return SkillState.Available;
    }

}
