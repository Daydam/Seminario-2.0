using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(ChargeWeapon))]
public class ChargeWeaponInspector : WeaponInspector
{
    ChargeWeapon _tgt;
    AnimationCurve _animCurve;

    public void OnEnable()
    {
        _tgt = (ChargeWeapon)target;
    }

    public override void ShowValues()
    {
        EditorGUILayout.BeginVertical();

        _tgt.bulletSpeed = EditorGUILayout.FloatField("Bullet Speed", _tgt.bulletSpeed);
        _tgt.maxCooldown = EditorGUILayout.FloatField("Cooldown to shoot", _tgt.maxCooldown);
        _tgt.minDamage = EditorGUILayout.FloatField("Minimum Damage", _tgt.minDamage);
        _tgt.maxDamage = EditorGUILayout.FloatField("Maximum Damage", _tgt.maxDamage);
        _tgt.maxChargeTime = EditorGUILayout.FloatField("Maximum charge time", _tgt.maxChargeTime);

        SetCurveValues();

        EditorGUILayout.LabelField("Damage by charge time");

        EditorGUILayout.CurveField(_animCurve);

        EditorGUILayout.EndVertical();
    }

    public override void SetValues()
    {

    }

    public override void SetCurveValues()
    {
        _animCurve = new AnimationCurve();
        var minChargeKey = new Keyframe(0, _tgt.minDamage, 0, 0);
        _animCurve.AddKey(minChargeKey);
        var maxChargeKey = new Keyframe(_tgt.maxChargeTime, _tgt.maxDamage, 0, 0);
        _animCurve.AddKey(maxChargeKey);
    }
}
