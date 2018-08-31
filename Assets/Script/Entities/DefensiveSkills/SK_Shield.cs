using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SK_Shield : DefensiveSkillBase
{
    public float maxCooldown;
    public float shieldDuration;
    Collider _shieldObj;
    Renderer _shieldRenderer;

    public float graphicActivationDelay = 1f;
    public float graphicDeactivationDelay = .3f;

    AnimationCurve _shieldActivationCurve;
    AnimationCurve _shieldDeactivationCurve;

    bool _isActive;
    float _currentCooldown = 0;
    float _shieldTimer = 0;

    protected override void Start()
    {
        base.Start();
        InitCurves();
        _shieldObj = GetComponentInChildren<Collider>(true);
        _shieldRenderer = _shieldObj.GetComponent<Renderer>();
        _shieldRenderer.material.SetFloat("_Activated", 0);
        _shieldObj.enabled = false;
    }

    void InitCurves()
    {
        _shieldActivationCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 0), new Keyframe(graphicActivationDelay, 1) });
        _shieldDeactivationCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0, 1), new Keyframe(graphicDeactivationDelay, 0) });
    }

    protected override void CheckInput()
    {
        if (_currentCooldown > 0) _currentCooldown -= Time.deltaTime;
        else if (control.DefensiveSkill() && !_isActive && !_owner.IsStunned && !_owner.IsDisarmed)
        {
            ActivateShield();
        }
    }
	
    void ActivateShield()
    {
        _shieldObj.enabled = true;
        _isActive = true;
        StartCoroutine(ApplyActivationFloat(_shieldActivationCurve));
    }

    void DeactivateShield(bool forceDeactivation = false)
    {
        _shieldObj.enabled = false;
        _isActive = false;
        if (forceDeactivation) _shieldRenderer.material.SetFloat("_Activated", 0);
        else StartCoroutine(ApplyActivationFloat(_shieldDeactivationCurve));

        _currentCooldown = 0;
        _shieldTimer = 0;
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
            DeactivateShield();
        }
    }

    public override void ResetRound()
    {
        DeactivateShield();
    }

    IEnumerator ApplyActivationFloat(AnimationCurve curve)
    {
        var elapsed = 0f;
        var timer = curve.keys.Last().time;

        while (elapsed <= timer)
        {
            var value = curve.Evaluate(elapsed);

            _shieldRenderer.material.SetFloat("_Activated", value);

            yield return new WaitForEndOfFrame();
            elapsed += Time.deltaTime;
        }
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
