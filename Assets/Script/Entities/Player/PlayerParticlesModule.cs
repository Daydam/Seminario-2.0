using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerParticlesModule : MonoBehaviour
{
    public GameObject stunStatusEffect;
    void Start ()
	{
		
	}
	
    public void ApplyStun(bool activation)
    {
        if (activation) stunStatusEffect.GetComponentInChildren<ParticleSystem>().Play();
        else stunStatusEffect.GetComponentInChildren<ParticleSystem>().Stop();       
    }
}
