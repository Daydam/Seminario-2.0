using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Shield : DefensiveSkillBase
{
    public float maxCooldown;
    public float shieldDuration;
    public GameObject shieldPrefab;

    bool _isActive;
    float _currentCooldown = 0;
    float _shieldTimer = 0;

    protected override void Start()
    {
        base.Start();
        shieldPrefab = transform.Find("Shield").gameObject;
    }

    protected override void CheckInput()
    {
        if (_currentCooldown > 0) _currentCooldown -= Time.deltaTime;
        else if (control.DefensiveSkill() && !_isActive)
        {
            shieldPrefab.SetActive(true);
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
            shieldPrefab.SetActive(false);
        }
    }
}
