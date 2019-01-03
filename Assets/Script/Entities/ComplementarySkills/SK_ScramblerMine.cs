using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SK_ScramblerMine : ComplementarySkillBase
{
    public float maxCooldown, duration, radius;

    float _currentCooldown = 0;
    DMM_ScramblerMine _mine;
    bool _canTap = true;
    bool _inUse;

    protected override void Start()
    {
        base.Start();
        var obj = Resources.Load<DMM_ScramblerMine>("Prefabs/Projectiles/ScramblerMine");
        _mine = Instantiate(obj, transform.position, Quaternion.identity);
        _mine.gameObject.SetActive(false);
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
                    if (activationAnim != null) activationAnim.Play();
                    _canTap = false;
                    if (MineActive()) _mine.Explode(true);

                    _mine.gameObject.SetActive(true);
                    _mine.Spawn(_owner.transform.position, _owner.gameObject.transform.forward, duration, radius, _owner.gameObject.tag);

                    _currentCooldown = maxCooldown;
                }
            }
            //else _stateSource.PlayOneShot(unavailableSound);
        }
        else _canTap = true;
    }

    public override void ResetRound()
    {
    	 if (MineActive()) _mine.Explode(true);

        _currentCooldown = 0;
    }

    bool MineActive()
    {
        return _mine != null && _mine.gameObject.activeInHierarchy;
    }

    public override SkillState GetActualState()
    {
        var unavailable = _currentCooldown > 0;
        var userDisabled = _owner.IsStunned || _owner.IsDisarmed;
        var active = MineActive();

        if (userDisabled) return SkillState.UserDisabled;
        else if (unavailable) return SkillState.Unavailable;
        else if (active) return SkillState.Active;
        else return SkillState.Available;
    }
}