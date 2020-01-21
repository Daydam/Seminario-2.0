using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using XInputDotNetPure;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    GamePadState[] previousGamePads;
    GamePadState[] currentGamePads;

    public UIButton[] buttons;
    int currentButton = 0;

    void Start ()
	{
        previousGamePads = new GamePadState[4];
        currentGamePads = new GamePadState[4];
        for (int i = 0; i < currentGamePads.Length; i++)
        {
            currentGamePads[i] = GamePad.GetState((PlayerIndex)i);
        }
        buttons[currentButton].SetHighlighted(true);
        //Maybe check if you're connected to internet, and enable/disable the Online Button. MAYBE.
    }

    void Update()
    {
        print(currentButton);
        for (int i = 0; i < currentGamePads.Length; i++)
        {
            previousGamePads[i] = currentGamePads[i];
            currentGamePads[i] = GamePad.GetState((PlayerIndex)i);
            if (-0.3f < JoystickInput.LeftAnalog(currentGamePads[i]).y && JoystickInput.LeftAnalog(currentGamePads[i]).y < 0.3f)
            {
                if (JoystickInput.LeftAnalog(currentGamePads[i]).y <= -0.3f)
                {
                    buttons[currentButton].SetHighlighted(false);
                    currentButton = currentButton == buttons.Length - 1 ? 0 : currentButton++;
                    buttons[currentButton].SetHighlighted(true);
                }
                else if (JoystickInput.LeftAnalog(currentGamePads[i]).y >= 0.3f)
                {
                    buttons[currentButton].SetHighlighted(false);
                    currentButton = currentButton == 0 ? buttons.Length - 1 : currentButton--;
                    buttons[currentButton].SetHighlighted(true);
                }
            }
            if (JoystickInput.allKeys[JoystickKey.A](previousGamePads[i], currentGamePads[i])
            || JoystickInput.allKeys[JoystickKey.START](previousGamePads[i], currentGamePads[i]))
            {
                buttons[currentButton].SetPressed(true);
            }
        }
        //Keyboard
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            buttons[currentButton].SetHighlighted(false);
            currentButton = Mathf.Min(currentButton+1, buttons.Length-1);
            buttons[currentButton].SetHighlighted(true);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            buttons[currentButton].SetHighlighted(false);
            currentButton = Mathf.Max(currentButton-1, 0);
            buttons[currentButton].SetHighlighted(true);
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            buttons[currentButton].SetPressed(true);
            //The most hardcoded hardcode ever hardcoded
            if (currentButton == 0) OnPlayLocal();
            else if (currentButton == 1) OnPlayOnline();
        }
    }

    //To be used by the Play Local Button
    public void OnPlayLocal()
    {
        print("Starting an online match!");
    }

    //To be used by the Play Online Button
    public void OnPlayOnline()
    {
        print("Starting an online match!");
    }
}
