using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(TapWeapon))]
public class TapWeaponInspector : WeaponInspector
{
    TapWeapon _tgt;

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


        EditorGUILayout.CurveField(_tgt.damageFalloff);

        EditorGUILayout.EndVertical();
    }

    public override void SetValues()
    {

    }

    public override void SetCurveValues()
    {
        _tgt.damageFalloff = new AnimationCurve();
        var initialKey = new Keyframe(0, _tgt.maxDamage, 0, 0);
        _tgt.damageFalloff.AddKey(initialKey);
        var startFalloff = new Keyframe(_tgt.falloffStart, _tgt.maxDamage, 0, 0);
        _tgt.damageFalloff.AddKey(startFalloff);
        var endtFalloff = new Keyframe(_tgt.falloffEnd, _tgt.minDamage, 0, 0);
        _tgt.damageFalloff.AddKey(endtFalloff);
    }
}
