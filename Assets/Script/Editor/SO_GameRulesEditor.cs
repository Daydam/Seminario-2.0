using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SO_GameRules))]
[CanEditMultipleObjects]
public class SO_GameRulesEditor : Editor
{
    SO_GameRules Target { get { return (SO_GameRules)target; } }
    GUIStyle titleStyle;

    public override void OnInspectorGUI()
    {
        if (titleStyle == null) titleStyle = new GUIStyle();
        titleStyle.fontStyle = FontStyle.Bold;

        SerializedObject gameRules = new SerializedObject(Target);
        gameRules.Update();

        EditorGUILayout.LabelField("Points awarded:", titleStyle);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Points for killing");
        EditorGUILayout.PropertyField(gameRules.FindProperty("pointsPerKill"));
        EditorGUILayout.LabelField("Points for being thrown off the arena");
        EditorGUILayout.PropertyField(gameRules.FindProperty("pointsPerDrop"));
        EditorGUILayout.LabelField("Points for being last");
        EditorGUILayout.PropertyField(gameRules.FindProperty("pointsForLast"));
        EditorGUILayout.LabelField("Points for suiciding");
        EditorGUILayout.PropertyField(gameRules.FindProperty("pointsPerSuicide"));
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Points required to win:", titleStyle);
        EditorGUILayout.Space();
        if (Target.pointsToWin == null) Target.pointsToWin = new int[3];
        for (int i = 0; i < 3; i++)
        {
            var property = gameRules.FindProperty("pointsToWin");
            EditorGUILayout.PropertyField(property.GetArrayElementAtIndex(i), new GUIContent((i + 2) + " Players"));
        }

        gameRules.ApplyModifiedProperties();
    }
}
