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

        EditorGUILayout.LabelField("Points awarded:", titleStyle);
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Points for killing");
        Target.pointsPerKill = EditorGUILayout.DelayedIntField(Target.pointsPerKill);
        EditorGUILayout.LabelField("Points for being thrown off the arena");
        Target.pointsPerDrop = EditorGUILayout.DelayedIntField(Target.pointsPerDrop);
        EditorGUILayout.LabelField("Points for being last");
        Target.pointsForLast = EditorGUILayout.DelayedIntField(Target.pointsForLast);
        EditorGUILayout.LabelField("Points for suiciding");
        Target.pointsPerSuicide = EditorGUILayout.DelayedIntField(Target.pointsPerSuicide);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Points required to win:", titleStyle);
        EditorGUILayout.Space();
        if (Target.pointsToWin == null) Target.pointsToWin = new int[3];
        for (int i = 0; i < 3; i++)
        {
            Target.pointsToWin[i] = EditorGUILayout.DelayedIntField((i+2) + " Players", Target.pointsToWin[i]);
        }
    }
}
