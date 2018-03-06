using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Key Bindings", menuName = "ScriptableObjects/Input/Key Bindings")]
public class KeyBindings : ScriptableObject
{
    public JoystickKey joystickPrimaryWeapon;
    public JoystickKey joystickSecondaryWeapon;
    public JoystickKey joystickPrimarySkill;
    public JoystickKey joystickSecondarySkill;
}