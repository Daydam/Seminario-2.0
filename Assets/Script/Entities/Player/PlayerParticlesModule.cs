using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerParticlesModule : MonoBehaviour
{
    public GameObject stunStatusEffect;
    ParticleSystem _stunStatusEffect;
    void Awake ()
	{
        _stunStatusEffect = stunStatusEffect.GetComponentInChildren<ParticleSystem>();
    }
	
    public void ApplyStun(bool activation)
    {
        if (activation) _stunStatusEffect.Play();
        else _stunStatusEffect.Stop();       
    }
}
