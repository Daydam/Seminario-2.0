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
    //We should add every necessary command here and in Controller.cs, in order to make it work. Once that's done, everything should work.
}