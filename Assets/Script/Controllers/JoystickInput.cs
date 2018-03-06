using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JoystickInput
{
    public static Dictionary<JoystickKey, Func<int, bool>> allKeys =
        new Dictionary<JoystickKey, Func<int, bool>>
        {
            //Agarraos de las bolas, pues este será el peor hardcodeo de la historia tio. Joder.
            { JoystickKey.A, a => Input.GetKey(KeyCode.Joystick1Button0 + (a - 1) * 20)},
            { JoystickKey.B, a => Input.GetKey(KeyCode.Joystick1Button1 + (a - 1) * 20)},
            { JoystickKey.X, a => Input.GetKey(KeyCode.Joystick1Button2 + (a - 1) * 20)},
            { JoystickKey.Y, a => Input.GetKey(KeyCode.Joystick1Button3 + (a - 1) * 20)},
            { JoystickKey.LB, a => Input.GetKey(KeyCode.Joystick1Button4 + (a - 1) * 20)},
            { JoystickKey.RB, a => Input.GetKey(KeyCode.Joystick1Button5 + (a - 1) * 20)},
            { JoystickKey.BACK, a => Input.GetKey(KeyCode.Joystick1Button6 + (a - 1) * 20)},
            { JoystickKey.START, a => Input.GetKey(KeyCode.Joystick1Button7 + (a - 1) * 20)},
            { JoystickKey.LEFT_STICK_CLICK, a => Input.GetKey(KeyCode.Joystick1Button8 + (a - 1) * 20)},
            { JoystickKey.RIGHT_STICK_CLICK, a => Input.GetKey(KeyCode.Joystick1Button9 + (a - 1) * 20)},
            { JoystickKey.LT, a => Input.GetAxis("Joystick " + a + " Axis 9") > 0},
            { JoystickKey.RT, a => Input.GetAxis("Joystick " + a + " Axis 10") > 0},
        };

    public static Vector2 LeftAnalog(int player)
    {
        return new Vector2(Input.GetAxisRaw("Joystick " + player + " Axis 1"), Input.GetAxisRaw("Joystick " + player + " Axis 2") * -1);
    }

    public static Vector2 RightAnalog(int player)
    {
        return new Vector2(Input.GetAxisRaw("Joystick " + player + " Axis 4"), Input.GetAxisRaw("Joystick " + player + " Axis 5") * -1);
    }
}

public enum JoystickKey
{
    A,
    B,
    X,
    Y,
    LB,
    LT,
    RB,
    RT,
    BACK,
    START,
    LEFT_STICK_CLICK,
    RIGHT_STICK_CLICK,
}
