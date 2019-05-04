using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MainMenu : MonoBehaviour
{
	void Start ()
	{
        //Maybe check if you're connected to internet, and enable/disable the Online Button. MAYBE.
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
