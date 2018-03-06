using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class Weapon : MonoBehaviour
{
    //INITIAL COMMENT: THIS CLASS IS ONLY HERE AS A REMINDER. THERE SHOULD BE A FUNCTION DETERMINING THE ACTION.
    //JUST ADD A BOOL TO SEE IF THE KEY HAS TO BE PRESSED AND RELEASED OR CAN BE KEPT PRESSED, AND THE FUNCTION SHOULD ACT FOR THAT.

    //CREATE A FEW CLASSES THAT INHERIT FROM THIS, AND MAKE THIS AN INTERFACE. CREATE "TapWeapon" and "AutomaticWeapon".
    //In each case, create cooldown timers to make sure no dumbass can spam bullets.

    //WE NEED SOME WAY TO DETECT IF THE WEAPON IS PRIMARY OR SECONDARY

    protected Controller control;
    protected Func<bool> inputMethod;

    void Start()
    {
        control = GetComponentInParent<Player>().Control;
        //I need some way to make the weapon recognize if it's primary or secondary.
        //Maybe player should have a list of its weapons and skills, so the weapon can look for itself.
    }

    void Update()
    {
        CheckInput();
    }

    protected abstract void CheckInput();

}
