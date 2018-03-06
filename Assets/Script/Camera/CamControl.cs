using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    public Vector3 cameraOffset;
	
	void Update ()
	{
        Vector3 camPos = new Vector3();

        foreach (Player p in GameManager.Instance.Players)
        {
            camPos += new Vector3(p.transform.position.x, 0,  p.transform.position.z);
        }
        camPos /= GameManager.Instance.Players.Count;

        transform.position = camPos + cameraOffset;
        transform.LookAt(camPos);
	}
}
