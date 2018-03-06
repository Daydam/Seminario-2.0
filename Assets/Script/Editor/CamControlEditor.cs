using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CamControl))]
public class CamControlEditor : Editor
{
    CamControl Target { get { return (CamControl)target; } }

    public override void OnInspectorGUI()
    {
        Target.maximumDistancePercentage = EditorGUILayout.Slider("Maximum Distance Percentage", Target.maximumDistancePercentage, 0, 1);
        Target.minimumDistance = EditorGUILayout.DelayedFloatField("Minimum Distance", Target.minimumDistance);
        Target.cameraOffset = EditorGUILayout.Vector3Field("Camera Offset", Target.cameraOffset);
        if (Target.cameraOffset == Vector3.zero) EditorGUILayout.HelpBox("The script won't work if the offset vector equals zero!", MessageType.Error);
        else if (Target.cameraOffset.x != 0) EditorGUILayout.HelpBox("We firmly suggest you keep X as 0, since having it otherwise would make the joystick movement feel 'crooked'", MessageType.Warning);
    }
}
