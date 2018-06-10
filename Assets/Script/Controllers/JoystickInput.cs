using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using XInputDotNetPure;

public class JoystickInput
{
    public static Dictionary<JoystickKey, Func<GamePadState, GamePadState, bool>> allKeys =
        new Dictionary<JoystickKey, Func<GamePadState, GamePadState, bool>>
        {
            //Agarraos de las bolas, pues este será el peor hardcodeo de la historia tio. Joder.
            //(a - 1 ) * 20 consigue el int de la tecla de cualquier jugador.
            //PELOTUDO
            { JoystickKey.A, (a,b) => a.Buttons.A == ButtonState.Released && b.Buttons.A == ButtonState.Pressed},
            { JoystickKey.B, (a,b) => a.Buttons.B == ButtonState.Released && b.Buttons.B == ButtonState.Pressed},
            { JoystickKey.X, (a,b) => a.Buttons.X == ButtonState.Released && b.Buttons.X == ButtonState.Pressed},
            { JoystickKey.Y, (a,b) => a.Buttons.Y == ButtonState.Released && b.Buttons.Y == ButtonState.Pressed},
            { JoystickKey.LB, (a,b) => a.Buttons.LeftShoulder == ButtonState.Released && b.Buttons.LeftShoulder == ButtonState.Pressed},
            { JoystickKey.RB, (a,b) => a.Buttons.RightShoulder == ButtonState.Released && b.Buttons.RightShoulder == ButtonState.Pressed},
            { JoystickKey.BACK, (a,b) => a.Buttons.Back == ButtonState.Released && b.Buttons.Back == ButtonState.Pressed},
            { JoystickKey.START, (a,b) => a.Buttons.Start == ButtonState.Released && b.Buttons.Start == ButtonState.Pressed},
            { JoystickKey.LEFT_STICK_CLICK, (a,b) => a.Buttons.LeftStick == ButtonState.Released && b.Buttons.LeftStick == ButtonState.Pressed},
            { JoystickKey.RIGHT_STICK_CLICK, (a,b) => a.Buttons.RightStick == ButtonState.Released && b.Buttons.RightStick == ButtonState.Pressed},
            { JoystickKey.LT, (a,b) => b.Triggers.Left > 0},
            { JoystickKey.RT, (a,b) => b.Triggers.Right > 0},
        };

    public static Vector2 LeftAnalog(GamePadState gamepad)
    {
        return new Vector2(gamepad.ThumbSticks.Left.X, gamepad.ThumbSticks.Left.Y);
    }

    public static Vector2 RightAnalog(GamePadState gamepad)
    {
        return new Vector2(gamepad.ThumbSticks.Right.X, gamepad.ThumbSticks.Right.Y);
    }
}

/// <summary>
/// Puto traga leche JAJ
/// </summary>
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
