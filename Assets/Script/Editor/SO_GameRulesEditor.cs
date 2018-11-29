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
    bool showHelp;

    public override void OnInspectorGUI()
    {
        if (titleStyle == null) titleStyle = new GUIStyle();
        titleStyle.fontStyle = FontStyle.Bold;

        SerializedObject gameRules = new SerializedObject(Target);
        gameRules.Update();

        if (GUILayout.Button("Show/Hide Help")) showHelp = !showHelp;

        EditorGUILayout.LabelField("Points awarded:", titleStyle);
        EditorGUILayout.Space();
        if(showHelp) EditorGUILayout.HelpBox("Points for killing", MessageType.Info);
        EditorGUILayout.PropertyField(gameRules.FindProperty("pointsPerKill"));
        if (showHelp) EditorGUILayout.HelpBox("Points for being thrown off the arena", MessageType.Info);
        EditorGUILayout.PropertyField(gameRules.FindProperty("pointsPerDrop"));
        if (showHelp) EditorGUILayout.HelpBox("Points for being last", MessageType.Info);
        EditorGUILayout.PropertyField(gameRules.FindProperty("pointsForLast"));
        if (showHelp) EditorGUILayout.HelpBox("Points for suiciding", MessageType.Info);
        EditorGUILayout.PropertyField(gameRules.FindProperty("pointsPerSuicide"));
        if (showHelp) EditorGUILayout.HelpBox("Points for dying", MessageType.Info);
        EditorGUILayout.PropertyField(gameRules.FindProperty("pointsPerDeath"));
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
