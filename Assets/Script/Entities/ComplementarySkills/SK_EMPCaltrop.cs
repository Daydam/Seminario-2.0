using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SK_EMPCaltrop : ComplementarySkillBase
{
    public SO_EMPCaltrop skillData;

    float _currentCooldown = 0;
    bool _canTap = true;

    List<DMM_EMPCaltrop> _charges = new List<DMM_EMPCaltrop>();

    protected override void Start()
    {
        base.Start();

        skillData = Resources.Load<SO_EMPCaltrop>("Scriptable Objects/Skills/Complementary/" + _owner.weightModule.prefix + GetSkillName() + _owner.weightModule.sufix) as SO_EMPCaltrop;


        LoadPrefabs();
    }

    void LoadPrefabs()
    {
        var loadedPrefab = Resources.Load<DMM_EMPCaltrop>("Prefabs/Projectiles/EMPCaltrop");

        for (byte i = 0; i < skillData.maxChargesActive; i++)
        {
            var charge = Instantiate(loadedPrefab);
            _charges.Add(charge);
            charge.gameObject.SetActive(false);
        }
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
                if (activationAnim != null) activationAnim.Play();
                _canTap = false;

                LaunchCaltrop();

                _currentCooldown = skillData.maxCooldown;
            }
        }
        //else _stateSource.PlayOneShot(unavailableSound);
    }

    void LaunchCaltrop()
    {
        if (_charges.Where(x => !x.gameObject.activeInHierarchy).Any())
        {
            _charges = _charges.OrderBy(x => x.gameObject.activeInHierarchy).ToList();
        }
        else _charges = _charges.OrderByDescending(x => x.ElapsedLifeTime).ToList();

        var caltrop = _charges.First();
        if (caltrop.gameObject.activeInHierarchy) caltrop.ForceActivation();
        caltrop.gameObject.SetActive(true);
        caltrop.Spawn(_owner.transform.position, _owner.gameObject.transform.forward, _owner.gameObject.tag, OnCaltropActivation, skillData);
    }

    void OnCaltropActivation(DMM_EMPCaltrop obj)
    {
        obj.gameObject.SetActive(false);
    }

    public override void ResetRound()
    {
        _currentCooldown = 0;
        foreach (var item in _charges.Where(x => x.gameObject.activeInHierarchy))
        {
            item.ForceActivation();
        }
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