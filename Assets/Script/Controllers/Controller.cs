using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller
{
    int player;

    /* In case I needed a 3D camera, I'd use this. If the camera is set to be top-down, then this should be disabled
    bool invertVertical;
    public bool InvertVertical { get { return invertVertical; } set { invertVertical = value; } }*/

    public JoystickKey joystickPrimaryWeapon;
    public JoystickKey joystickSecondaryWeapon;
    public JoystickKey joystickPrimarySkill;
    public JoystickKey joystickSecondarySkill;
    //We could add anything here

    public Controller(int player)
    {
        this.player = player;
        var keyBindings = Resources.Load<KeyBindings>("Scriptable Objects/Key Bindings");
        joystickPrimaryWeapon = keyBindings.joystickPrimaryWeapon;
        joystickSecondaryWeapon = keyBindings.joystickSecondaryWeapon;
        joystickPrimarySkill = keyBindings.joystickPrimarySkill;
        joystickSecondarySkill = keyBindings.joystickSecondarySkill;
    }

    public Vector2 LeftAnalog()
    {
        return JoystickInput.LeftAnalog(player);
    }

    public Vector2 RightAnalog()
    {
        return JoystickInput.RightAnalog(player);
    }

    public bool PrimaryWeapon()
    {

        return JoystickInput.allKeys[joystickPrimaryWeapon](player);
    }

    public bool SecondaryWeapon()
    {
        return JoystickInput.allKeys[joystickSecondaryWeapon](player);
    }

    public bool PrimarySkill()
    {
        return JoystickInput.allKeys[joystickPrimarySkill](player);
    }

    public bool SecondarySkill()
    {
        return JoystickInput.allKeys[joystickSecondarySkill](player);
    }
}
