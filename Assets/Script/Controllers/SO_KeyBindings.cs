using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Key Bindings", menuName = "Scriptable Objects/Configuration/Input/Key Bindings")]
public class SO_KeyBindings : ScriptableObject
{
    public JoystickKey mainWeapon;
    public JoystickKey defensiveSkill;
    public JoystickKey complimentarySkill1;
    public JoystickKey complimentarySkill2;
    //We should add every necessary command here and in Controller.cs, in order to make it work. Once that's done, everything should work.
}