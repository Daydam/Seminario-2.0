using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(Weapon))]
public class WeaponInspector : Editor
{
    /*Weapon _target;

    public virtual void OnEnable()
    {
        _target = (Weapon)target;
    }*/

    public override void OnInspectorGUI()
    {
        ShowValues();
        SetValues();

        Repaint();
    }

    public virtual void ShowValues()
    {
    }

    public virtual void SetValues()
    {
    }

    public virtual void SetCurveValues()
    {
    }
}
