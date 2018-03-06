using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(KeyBindings))]
public class KeyBindingsEditor : Editor
{
    KeyBindings tg;

    void OnEnable()
    {
        tg = (KeyBindings)target;
    }

    public override void OnInspectorGUI()
    {
        tg.joystickPrimaryWeapon =  (JoystickKey)EditorGUILayout.EnumPopup("Primary Weapon", tg.joystickPrimaryWeapon);
        tg.joystickSecondaryWeapon = (JoystickKey)EditorGUILayout.EnumPopup("Secondary Weapon", tg.joystickSecondaryWeapon);
        tg.joystickPrimarySkill = (JoystickKey)EditorGUILayout.EnumPopup("Primary Skill", tg.joystickPrimarySkill);
        tg.joystickSecondarySkill = (JoystickKey)EditorGUILayout.EnumPopup("Secondary Skill", tg.joystickSecondarySkill);
    }
}
