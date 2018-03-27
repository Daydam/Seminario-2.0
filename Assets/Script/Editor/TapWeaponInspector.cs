using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(TapWeapon))]
public class TapWeaponInspector : WeaponInspector
{
    TapWeapon _tgt;
    AnimationCurve _animCurve;

    public void OnEnable()
    {
        _tgt = (TapWeapon)target;
    }

    public override void ShowValues()
    {
        EditorGUILayout.BeginVertical();

        _tgt.bulletSpeed = EditorGUILayout.FloatField("Bullet Speed", _tgt.bulletSpeed);
        _tgt.maxCooldown = EditorGUILayout.FloatField("Cooldown to shoot", _tgt.maxCooldown);
        _tgt.minDamage = EditorGUILayout.FloatField("Minimum Damage", _tgt.minDamage);
        _tgt.maxDamage = EditorGUILayout.FloatField("Maximum Damage", _tgt.maxDamage);
        _tgt.falloffStart = EditorGUILayout.FloatField("Damage Falloff start", _tgt.falloffStart);
        _tgt.falloffEnd = EditorGUILayout.FloatField("Damage Falloff end", _tgt.falloffEnd);

        SetCurveValues();

        EditorGUILayout.LabelField("Damage falloff by distance");


        EditorGUILayout.CurveField(_animCurve);

        EditorGUILayout.EndVertical();
    }

    public override void SetValues()
    {

    }

    public override void SetCurveValues()
    {
        _animCurve = new AnimationCurve();
        var initialKey = new Keyframe(0, _tgt.maxDamage, 0, 0);
        _animCurve.AddKey(initialKey);
        var startFalloff = new Keyframe(_tgt.falloffStart, _tgt.maxDamage, 0, 0);
        _animCurve.AddKey(startFalloff);
        var endtFalloff = new Keyframe(_tgt.falloffEnd, _tgt.minDamage, 0, 0);
        _animCurve.AddKey(endtFalloff);
    }
}
