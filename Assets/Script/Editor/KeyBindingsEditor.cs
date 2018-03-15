using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SO_KeyBindings))]
public class KeyBindingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(serializedObject.FindProperty("mainWeapon"), new GUIContent("Main Weapon"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("defensiveSkill"), new GUIContent("Defensive Skill"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("complimentarySkill1"), new GUIContent("Complimentary Skill 1"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("complimentarySkill2"), new GUIContent("Complimentary Skill 2"));
        serializedObject.ApplyModifiedProperties();
    }
}
