using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellerColor : MonoBehaviour
{
    Player owner;
    Renderer rend;

	void Start ()
	{
        owner = GetComponentInParent<Player>();
        rend = GetComponent<Renderer>();
	}
	
	void Update ()
	{
        rend.material.SetFloat("_Life", owner.LightsModule.GetLifeValue());
    }
}
