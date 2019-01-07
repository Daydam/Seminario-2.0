using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[ExecuteInEditMode]
public static class Catarsis
{
    public static void DoCatarsis()
    {
        Debug.Log("PABLO LA CONCHA DE TU MADRE");
    }

    public static void ForceCatarsis()
    {
        if (Input.GetKeyDown(KeyCode.P) && Input.GetKeyDown(KeyCode.A) && Input.GetKeyDown(KeyCode.B) && Input.GetKeyDown(KeyCode.L) && Input.GetKeyDown(KeyCode.O))
        {
            DoCatarsis();
        }
    }
}
