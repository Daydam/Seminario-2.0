using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SK_Shield : DefensiveSkillBase
{
    public float maxCooldown;
    public float shieldDuration;
    GameObject _shieldObj;

    bool _isActive;
    float _currentCooldown = 0;
    float _shieldTimer = 0;

    protected override void Start()
    {
        base.Start();
        _shieldObj = transform.Find("Shield").gameObject;
    }

    protected override void CheckInput()
    {
        if (_currentCooldown > 0) _currentCooldown -= Time.deltaTime;
        else if (control.DefensiveSkill() && !_isActive && !_owner.IsStunned && !_owner.IsDisarmed)
        {
            _shieldObj.SetActive(true);
            _isActive = true;
        }
    }
	
	protected override void Update()
	{
        base.Update();
        if (_isActive) ManageShield();
	}

    void ManageShield()
    {
        _shieldTimer += Time.deltaTime;

        if (_shieldTimer >= shieldDuration)
        {
            _isActive = false;
            _currentCooldown = maxCooldown;
            _shieldTimer = 0;
            _shieldObj.SetActive(false);
        }
    }

    public override void ResetRound()
    {
        _currentCooldown = 0;
        _isActive = false;
        _shieldTimer = 0;
    }

    public override SkillState GetActualState()
    {
        var unavailable = _currentCooldown > 0;
        var userDisabled = _owner.IsStunned || _owner.IsDisarmed;
        var active = _isActive;

        if (userDisabled) return SkillState.UserDisabled;
        else if (unavailable) return SkillState.Unavailable;
        else if (active) return SkillState.Active;
        else return SkillState.Available;
    }
}
