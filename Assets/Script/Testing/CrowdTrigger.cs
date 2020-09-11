using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdTrigger : MonoBehaviour
{
    CrowdMaster master;

	void Start ()
	{
        master = FindObjectOfType<CrowdMaster>();
	}
	
	void Update ()
	{
        if (Input.GetKeyDown(KeyCode.C)) master.Cheer();
        if (Input.GetKeyDown(KeyCode.B)) master.Boo();
        if (Input.GetKeyDown(KeyCode.S)) master.Surprise();
    }
}
