using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(KeyBindings))]
public class KeyBindingsEditor : Editor
{
    KeyBindings Target { get { return (KeyBindings)target; } }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("joystickPrimaryWeapon"), new GUIContent("Primary Weapon"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("joystickSecondaryWeapon"), new GUIContent("Secondary Weapon"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("joystickPrimarySkill"), new GUIContent("Primary Skill"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("joystickSecondarySkill"), new GUIContent("Secondary Skill"));
        serializedObject.ApplyModifiedProperties();
    }
}
