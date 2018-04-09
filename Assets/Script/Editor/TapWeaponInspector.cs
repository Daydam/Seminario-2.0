using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

//[CustomEditor(typeof(TapWeapon))]
public class TapWeaponInspector : Editor
{
    TapWeapon _tgt;

    public void OnEnable()
    {
        _tgt = (TapWeapon)target;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();

        ShowValues();

        Repaint();
        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
    }


    public void ShowValues()
    {
        EditorGUILayout.BeginVertical();

        _tgt.bulletSpeed = EditorGUILayout.FloatField("Bullet Speed", _tgt.bulletSpeed);
        _tgt.maxCooldown = EditorGUILayout.IntSlider("Cooldown to shoot", _tgt.maxCooldown, 1, 10);
        _tgt.minDamage = EditorGUILayout.FloatField("Minimum Damage", _tgt.minDamage);
        _tgt.maxDamage = EditorGUILayout.FloatField("Maximum Damage", _tgt.maxDamage);
        _tgt.falloffStart = EditorGUILayout.FloatField("Damage Falloff start", _tgt.falloffStart);
        _tgt.falloffEnd = EditorGUILayout.FloatField("Damage Falloff end", _tgt.falloffEnd);

        EditorGUILayout.LabelField("Damage falloff by distance");


        EditorGUILayout.CurveField(_tgt.damageFalloff);
        SetCurveValues();

        EditorGUILayout.EndVertical();
    }

    public void SetCurveValues()
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
