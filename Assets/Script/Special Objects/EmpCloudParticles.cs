using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Events;
using System;

public class EmpCloudParticles : MonoBehaviour
{
    ParticleSystem particles;
    float startingRadius;

    void Start()
    {
        particles = GetComponent<ParticleSystem>();
        EventManager.Instance.AddEventListener(GameEvents.RoundReset, OnRoundReset);
        startingRadius = particles.shape.radius;
    }

    void OnRoundReset(object[] paramsContainer)
    {
        particles.Clear();
        var temp = particles.shape;
        temp.radius = startingRadius;
        particles.Play();
    }

    void Update ()
	{
        var asd = particles.shape;
        asd.radius = transform.parent.lossyScale.x /2;
	}
}
