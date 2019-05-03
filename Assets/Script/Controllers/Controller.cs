using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class Controller
{
    //Goes from 0 to 3!
    PlayerIndex player;
    GamePadState state;
    GamePadState prevState;
    bool keyboardEnabled;

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
        if((int)player == 3)
        {
            if (Input.GetKey(KeyCode.F) && Input.GetKey(KeyCode.Return)) keyboardEnabled = true;
            if (Input.GetKey(KeyCode.F) && Input.GetKey(KeyCode.Escape)) keyboardEnabled = false;
        }
    }

    public void SetVibration(float lowFrequencyIntensity, float highFrequencyIntensity)
    {
        GamePad.SetVibration(player, lowFrequencyIntensity, highFrequencyIntensity);
    }

    public Vector2 LeftAnalog()
    {
        if(keyboardEnabled)
        {
            int xValue = 0;
            if (Input.GetKey(KeyCode.A)) value--;
            if (Input.GetKey(KeyCode.D)) value++;
            int yValue = 0;
            if (Input.GetKey(KeyCode.S)) value--;
            if (Input.GetKey(KeyCode.W)) value++;
            return new Vector2(xValue, yValue);
        }
        return JoystickInput.LeftAnalog(state);
    }

    public Vector2 RightAnalog()
    {
        if (keyboardEnabled) return new Vector2(Input.GetAxis("Mouse X"), 0);
        else return JoystickInput.RightAnalog(state);
    }

    public bool MainWeapon()
    {
        if (keyboardEnabled) return Input.GetMouseButtonDown(0);
        else return JoystickInput.allKeys[mainWeapon](prevState, state);
    }

    public bool DefensiveSkill()
    {
        if (keyboardEnabled) return Input.GetMouseButtonDown(1);
        else return JoystickInput.allKeys[defensiveSkill](prevState, state);
    }

    public bool ComplimentarySkill1()
    {
        if (keyboardEnabled) return Input.GetKeyDown(KeyCode.Q);
        else return JoystickInput.allKeys[complimentarySkill1](prevState, state);
    }

    public bool ComplimentarySkill2()
    {
        if (keyboardEnabled) return Input.GetKeyDown(KeyCode.E);
        else return JoystickInput.allKeys[complimentarySkill2](prevState, state);
    }
}
