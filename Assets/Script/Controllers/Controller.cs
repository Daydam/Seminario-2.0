using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class Controller
{
    PlayerIndex player;
    GamePadState state;
    GamePadState prevState;

    /* In case I needed a 3D camera, I'd use this. If the camera is set to be top-down, then this should be disabled
    bool invertVertical;
    public bool InvertVertical { get { return invertVertical; } set { invertVertical = value; } }*/

    public JoystickKey mainWeapon;
    public JoystickKey defensiveSkill;
    public JoystickKey complimentarySkill1;
    public JoystickKey complimentarySkill2;
    //We could add anything here

    public Controller(int player)
    {
        this.player = (PlayerIndex)player;
        var keyBindings = Resources.Load<SO_KeyBindings>("Scriptable Objects/Key Bindings");
        mainWeapon = keyBindings.mainWeapon;
        defensiveSkill = keyBindings.defensiveSkill;
        complimentarySkill1 = keyBindings.complimentarySkill1;
        complimentarySkill2 = keyBindings.complimentarySkill2;
    }

    public void UpdateState()
    {
        //XInput
        prevState = state;
        state = GamePad.GetState(player);
    }

    public Vector2 LeftAnalog()
    {
        return JoystickInput.LeftAnalog(state);
    }

    public Vector2 RightAnalog()
    {
        return JoystickInput.RightAnalog(state);
    }

    public bool MainWeapon()
    {

        return JoystickInput.allKeys[mainWeapon](prevState, state);
    }

    public bool DefensiveSkill()
    {
        return JoystickInput.allKeys[defensiveSkill](prevState, state);
    }

    public bool ComplimentarySkill1()
    {
        return JoystickInput.allKeys[complimentarySkill1](prevState, state);
    }

    public bool ComplimentarySkill2()
    {
        return JoystickInput.allKeys[complimentarySkill2](prevState, state);
    }
}
