using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmpCloudParticles : MonoBehaviour
{
    ParticleSystem particles;

    void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    void Update ()
	{
        var asd = particles.shape;
        asd.radius = transform.parent.lossyScale.x /2;
	}
}
