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

    public Button localPlay;
    public Button onlinePlay;

    void Start ()
	{
        previousGamePads = new GamePadState[4];
        currentGamePads = new GamePadState[4];
        for (int i = 0; i < currentGamePads.Length; i++)
        {
            currentGamePads[i] = GamePad.GetState((PlayerIndex)i);
        }

        //Maybe check if you're connected to internet, and enable/disable the Online Button. MAYBE.
    }

    void Update()
    {
        for (int i = 0; i < currentGamePads.Length; i++)
        {
            previousGamePads[i] = currentGamePads[i];
            currentGamePads[i] = GamePad.GetState((PlayerIndex)i);
            if(-0.3f < JoystickInput.LeftAnalog(currentGamePads[i]).y && JoystickInput.LeftAnalog(currentGamePads[i]).y < 0.3f)
            {
                if (JoystickInput.LeftAnalog(currentGamePads[i]).y <= -0.3f)
                {
                    //Select first button. Do we simply change colors and say "Fuck it?" Yeah, that works.
                }
                else if (JoystickInput.LeftAnalog(currentGamePads[i]).y >= 0.3f)
                {

                }
            }
            if(Input.GetKeyDown(KeyCode.DownArrow))
            {

            }
            else if(Input.GetKeyDown(KeyCode.UpArrow))
            {

            }
            else if(Input.GetKeyDown(KeyCode.Return) || (JoystickInput.allKeys[JoystickKey.A](previousGamePads[i], currentGamePads[i])
            || JoystickInput.allKeys[JoystickKey.START](previousGamePads[i], currentGamePads[i])))
            {

            }
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
