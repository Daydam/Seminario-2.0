using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using XInputDotNetPure;

public static class AnyButtonHandler
{
    public static bool AnyButtonPressed()
    {
        return
            JoystickAnyButton(PlayerIndex.One) ||
            JoystickAnyButton(PlayerIndex.Two) ||
            JoystickAnyButton(PlayerIndex.Three) ||
            JoystickAnyButton(PlayerIndex.Four);
    }

    static bool JoystickAnyButton(PlayerIndex indx)
    {
        var cond =
            GamePad.GetState(indx).Buttons.A == ButtonState.Pressed ||
            GamePad.GetState(indx).Buttons.B == ButtonState.Pressed ||
            GamePad.GetState(indx).Buttons.X == ButtonState.Pressed ||
            GamePad.GetState(indx).Buttons.Y == ButtonState.Pressed ||
            GamePad.GetState(indx).Buttons.Back == ButtonState.Pressed ||
            GamePad.GetState(indx).Buttons.Start == ButtonState.Pressed ||
            GamePad.GetState(indx).Buttons.LeftShoulder == ButtonState.Pressed ||
            GamePad.GetState(indx).Buttons.LeftStick == ButtonState.Pressed ||
            GamePad.GetState(indx).Buttons.RightShoulder == ButtonState.Pressed ||
            GamePad.GetState(indx).Buttons.RightStick == ButtonState.Pressed ||
            GamePad.GetState(indx).Triggers.Left != 0 ||
            GamePad.GetState(indx).Triggers.Right != 0;

        return cond;
    }
}
